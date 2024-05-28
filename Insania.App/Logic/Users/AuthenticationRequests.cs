using System.Net;
using System.Text.Json;

using Microsoft.Extensions.Configuration;

using Insania.BusinessLogic.Users.Authentication;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Models.Users.Authentication;

namespace Insania.App.Logic.Users;

/// <summary>
/// Запросы аутентификации
/// </summary>
/// <param name="configuration">Интерфейс конфигурации</param>
public class AuthenticationRequests(IConfiguration configuration) : IAuthentication
{
    /// <summary>
    /// Интерфейс конфигурации
    /// </summary>
    private readonly IConfiguration _configuration = configuration;

    /// <summary>
    /// Параметры json
    /// </summary>
    private readonly JsonSerializerOptions _settings = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// Метод аутентификации по логину и паролю
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль</param>
    /// <returns></returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<AuthenticationResponse> Login(string? login, string? password)
    {
        try
        {
            //Проверяем данные из файла конфигурации
            if (string.IsNullOrWhiteSpace(_configuration["Api:Url"])) throw new InnerException(Errors.EmptyUrl);
            if (string.IsNullOrWhiteSpace(_configuration["Api:Version"])) throw new InnerException(Errors.EmptyVersion);
            if (string.IsNullOrWhiteSpace(_configuration["Api:Authentication"])) throw new InnerException(Errors.EmptyUrlAuthentication);

            //Проверяем входные данные
            if (string.IsNullOrWhiteSpace(login)) throw new InnerException(Errors.EmptyLogin);
            if (string.IsNullOrWhiteSpace(password)) throw new InnerException(Errors.EmptyPassword);

            //Формируем ссылку запроса
            string url = _configuration["Api:Url"] + _configuration["Api:Version"] + _configuration["Api:Authentication"] + string.Format("login?login={0}&password={1}", login, password);

            //Формируем клиента
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            using var client = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = delegate { return true; },
            });

            //Получаем данные по запросу
            using var result = await client.GetAsync(url) ?? throw new InnerException(Errors.EmptyResponse);

            //Если пришёл статус - Неавторизован, возвращаем исключение об этом
            if (result.StatusCode == HttpStatusCode.Unauthorized) throw new InnerException(Errors.IncorrectToken);

            //Если пришёл статус - не успешно, возвращаем исключение об этом
            if (result.StatusCode != HttpStatusCode.OK && result.StatusCode != HttpStatusCode.BadRequest) throw new InnerException(Errors.ServerError);

            //Десериализуем ответ
            var content = await result.Content.ReadAsStringAsync();
            AuthenticationResponse? response = JsonSerializer.Deserialize<AuthenticationResponse>(content, _settings) ?? throw new InnerException(Errors.EmptyResponse);

            //Если результат неуспешный и есть ошибка, возвращаем её
            if (!response.Success && response.Error != null && !string.IsNullOrWhiteSpace(response.Error.Message)) throw new InnerException(response.Error.Message!);

            //Если результат неуспешный и нет ошибка, возвращаем общее исключение
            if (!response.Success && response.Error != null) throw new InnerException(Errors.ServerError);

            //Если результат успешный и нет токена, возвращаем соответствующую ошибку
            if (response.Success && string.IsNullOrWhiteSpace(response.Token)) throw new InnerException(Errors.EmptyToken);

            //Записываем токен
            await SecureStorage.Default.SetAsync("token", response.Token!);

            //Возвращаем ответ
            return response;
        }
        catch (InnerException)
        {
            throw;
        }
        catch (Exception)
        {
            //Возвращаем общее исключение
            throw new Exception(Errors.ServerError);
        }
    }
}