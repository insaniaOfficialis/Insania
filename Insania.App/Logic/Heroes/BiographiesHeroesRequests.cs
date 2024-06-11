using System.Collections.Specialized;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

using Microsoft.Extensions.Configuration;

using Insania.BusinessLogic.Heroes.BiographiesHeroes;
using Insania.Models.Heroes.BiographiesHeroes;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.App.Logic.Heroes;

/// <summary>
/// Запросы работы с биографиями персонажей
/// </summary>
/// <param name="configuration">Интерфейс конфигурации</param>
public class BiographiesHeroesRequests(IConfiguration configuration) : IBiographiesHeroes
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
    /// Метод получения биографий персонажа
    /// </summary>
    /// <param name="heroId">Персонаж</param>
    /// <returns cref="GetBiographiesHeroResponseList">Модель ответа получения биографий персонажа</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<GetBiographiesHeroResponseList> GetList(long? heroId)
    {
        try
        {
            //Проверяем данные из файла конфигурации
            if (string.IsNullOrWhiteSpace(_configuration["Api:Url"])) throw new InnerException(Errors.EmptyUrl);
            if (string.IsNullOrWhiteSpace(_configuration["Api:Version"])) throw new InnerException(Errors.EmptyVersion);
            if (string.IsNullOrWhiteSpace(_configuration["Api:BiographiesHeroes"])) throw new InnerException(Errors.EmptyUrlBiographiesHeroes);

            //Проверяем входные данные
            if ((heroId ?? 0) == 0) throw new InnerException(Errors.EmptyRequest);

            //Получаем токен
            _token = await SecureStorage.Default.GetAsync("token");
            if (string.IsNullOrWhiteSpace(_token)) throw new InnerException(Errors.EmptyToken);

            //Формируем ссылку запроса
            NameValueCollection queryParam = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryParam.Add("heroId", heroId.ToString());
            string? param = queryParam?.ToString();
            string url = _configuration["Api:Url"] + _configuration["Api:Version"] + _configuration["Api:BiographiesHeroes"] 
                + "list" + (!string.IsNullOrWhiteSpace(param) ? $"?{param}" : "");

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
            GetBiographiesHeroResponseList? response = JsonSerializer.Deserialize<GetBiographiesHeroResponseList>(content, _settings) ?? throw new InnerException(Errors.EmptyResponse);

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