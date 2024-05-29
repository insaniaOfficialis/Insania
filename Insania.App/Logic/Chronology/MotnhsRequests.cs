using System.Net;
using System.Text.Json;

using Microsoft.Extensions.Configuration;

using Insania.BusinessLogic.Chronology.Months;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.App.Logic.Chronology;

/// <summary>
/// Запросы работы с месяцами
/// </summary>
/// <param name="configuration">Интерфейс конфигурации</param>
public class MonthsRequests(IConfiguration configuration) : IMonths
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
    /// Метод получения списка месяцев
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<BaseResponseList> GetMonthsList()
    {
        try
        {
            //Проверяем данные из файла конфигурации
            if (string.IsNullOrWhiteSpace(_configuration["Api:Url"])) throw new InnerException(Errors.EmptyUrl);
            if (string.IsNullOrWhiteSpace(_configuration["Api:Version"])) throw new InnerException(Errors.EmptyVersion);
            if (string.IsNullOrWhiteSpace(_configuration["Api:Months"])) throw new InnerException(Errors.EmptyUrlMonths);

            //Формируем ссылку запроса
            string url = _configuration["Api:Url"] + _configuration["Api:Version"] + _configuration["Api:Months"] + "list";

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