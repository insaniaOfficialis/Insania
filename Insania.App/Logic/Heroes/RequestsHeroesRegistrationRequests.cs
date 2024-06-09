using System.Collections.Specialized;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

using Microsoft.Extensions.Configuration;

using Insania.BusinessLogic.Heroes.RequestsHeroesRegistration;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Models.Heroes.RequestsHeroesRegistration;

namespace Insania.App.Logic.Heroes;

/// <summary>
/// Запросы работы с заявками на регистрацию персонажей
/// </summary>
/// <param name="configuration">Интерфейс конфигурации</param>
public class RequestsHeroesRegistrationRequests(IConfiguration configuration) : IRequestsHeroesRegistration
{
    /// <summary>
    /// Интерфейс конфигурации
    /// </summary>
    private readonly IConfiguration _configuration = configuration;

    /// <summary>
    /// Токен доступа
    /// </summary>
    private string? _token;

    /// <summary>
    /// Параметры json
    /// </summary>
    private readonly JsonSerializerOptions _settings = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// Метод получения заявки на регистрацию персонажа по id
    /// </summary>
    /// <param name="id">Первичный ключ заявки</param>
    /// <returns cref="GetRequestRegistrationHeroResponse">Ответ</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<GetRequestRegistrationHeroResponse> GetById(long? id)
    {
        try
        {
            //Проверяем данные из файла конфигурации
            if (string.IsNullOrWhiteSpace(_configuration["Api:Url"])) throw new InnerException(Errors.EmptyUrl);
            if (string.IsNullOrWhiteSpace(_configuration["Api:Version"])) throw new InnerException(Errors.EmptyVersion);
            if (string.IsNullOrWhiteSpace(_configuration["Api:RequestsHeroesRegistration"])) throw new InnerException(Errors.EmptyUrlRequestsHeroesRegistration);

            //Проверяем входные данные
            if ((id ?? 0) == 0) throw new InnerException(Errors.EmptyRequest);

            //Получаем токен
            _token = await SecureStorage.Default.GetAsync("token");
            if (string.IsNullOrWhiteSpace(_token)) throw new InnerException(Errors.EmptyToken);

            //Формируем ссылку запроса
            NameValueCollection queryParam = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryParam.Add("id", id.ToString());
            string? param = queryParam?.ToString();
            string url = _configuration["Api:Url"] + _configuration["Api:Version"] + _configuration["Api:RequestsHeroesRegistration"]
                + "byId" + (!string.IsNullOrWhiteSpace(param) ? $"?{param}" : "");

            //Формируем клиента
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            using var client = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = delegate { return true; },
            });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Replace("Bearer ", ""));

            //Получаем данные по запросу
            using var result = await client.GetAsync(url) ?? throw new InnerException(Errors.EmptyResponse);

            //Если пришёл статус - Неавторизован, возвращаем исключение об этом
            if (result.StatusCode == HttpStatusCode.Unauthorized) throw new InnerException(Errors.IncorrectToken);

            //Если пришёл статус - не успешно, возвращаем исключение об этом
            if (result.StatusCode != HttpStatusCode.OK && result.StatusCode != HttpStatusCode.BadRequest) throw new InnerException(Errors.ServerError);

            //Десериализуем ответ
            var content = await result.Content.ReadAsStringAsync();
            GetRequestRegistrationHeroResponse? response = JsonSerializer.Deserialize<GetRequestRegistrationHeroResponse>(content, _settings) ?? throw new InnerException(Errors.EmptyResponse);

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