using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Configuration;

using Insania.BusinessLogic.Users.Users;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Models.Users.Authentication;
using Insania.Models.OutCategories.Base;
using Insania.Models.Users.Users;

namespace Insania.App.Logic.Users;

/// <summary>
/// Запросы работы с пользователями
/// </summary>
/// <param name="configuration">Интерфейс конфигурации</param>
public class UsersRequests(IConfiguration configuration) : IUsers
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
    /// Метод добавления пользователя
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <returns></returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<BaseResponse> AddUser(AddUserRequest? request)
    {
        try
        {
            //Проверяем данные из файла конфигурации
            if (string.IsNullOrWhiteSpace(_configuration["Api:Url"])) throw new InnerException(Errors.EmptyUrl);
            if (string.IsNullOrWhiteSpace(_configuration["Api:Version"])) throw new InnerException(Errors.EmptyVersion);
            if (string.IsNullOrWhiteSpace(_configuration["Api:Users"])) throw new InnerException(Errors.EmptyUrlUsers);

            //Проверяем входные данные
            if (request == null) throw new InnerException(Errors.EmptyRequest);

            //Формируем ссылку запроса
            string url = _configuration["Api:Url"] + _configuration["Api:Version"] + _configuration["Api:Users"];

            //Формируем клиента
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            using var client = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = delegate { return true; },
            });
            string requestJson = JsonSerializer.Serialize(request);

            //Получаем данные по запросу
            using var result = await client.PostAsync(url, new StringContent(requestJson, Encoding.UTF8, "application/json")) ?? throw new InnerException(Errors.EmptyResponse);

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

    /// <summary>
    /// Метод проверки доступности логина
    /// </summary>
    /// <param name="login">Логин</param>
    /// <returns></returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<BaseResponse> CheckLogin(string? login)
    {
        try
        {
            //Проверяем данные из файла конфигурации
            if (string.IsNullOrWhiteSpace(_configuration["Api:Url"])) throw new InnerException(Errors.EmptyUrl);
            if (string.IsNullOrWhiteSpace(_configuration["Api:Version"])) throw new InnerException(Errors.EmptyVersion);
            if (string.IsNullOrWhiteSpace(_configuration["Api:Users"])) throw new InnerException(Errors.EmptyUrlUsers);

            //Проверяем входные данные
            if (string.IsNullOrWhiteSpace(login)) throw new InnerException(Errors.EmptyLogin);

            //Формируем ссылку запроса
            NameValueCollection queryParam = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryParam.Add("login", login);
            string? param = queryParam?.ToString();
            string url = _configuration["Api:Url"] + _configuration["Api:Version"] + _configuration["Api:Users"] + "checkLogin" +
                (!string.IsNullOrWhiteSpace(param) ? $"?{param}" : "");

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
            BaseResponseList? response = JsonSerializer.Deserialize<BaseResponseList>(content, _settings) ?? throw new InnerException(Errors.EmptyResponse);

            //Если результат неуспешный и есть ошибка, возвращаем её
            if (!response.Success && response.Error != null && !string.IsNullOrWhiteSpace(response.Error.Message)) throw new InnerException(response.Error.Message!);

            //Если результат неуспешный и нет ошибка, возвращаем общее исключение
            if (!response.Success && response.Error != null) throw new InnerException(Errors.ServerError);

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