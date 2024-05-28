using System.Net;
using System.Net.Http.Headers;

using Microsoft.Extensions.Configuration;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.BusinessLogic.OutOfCategories.CheckConnection;

namespace Insania.App.Logic.OutCategories;

/// <summary>
/// Запросы проверки соединения
/// </summary>
/// <param name="configuration">Интерфейс конфигурации</param>
public class CheckConnectionRequests(IConfiguration configuration) : ICheckConnection
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
    /// Метод проверки неавторизованного пользователя
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<bool> CheckAuthorize()
    {
        try
        {
            //Проверяем данные из файла конфигурации
            if (string.IsNullOrWhiteSpace(_configuration["Api:Url"])) throw new InnerException(Errors.EmptyUrl);
            if (string.IsNullOrWhiteSpace(_configuration["Api:Version"])) throw new InnerException(Errors.EmptyVersion);
            if (string.IsNullOrWhiteSpace(_configuration["Api:Check"])) throw new InnerException(Errors.EmptyUrlCheckConnection);

            //Получаем токен
            _token = await SecureStorage.Default.GetAsync("token");
            if (string.IsNullOrWhiteSpace(_token)) throw new InnerException(Errors.EmptyToken);

            //Формируем ссылку запроса
            string url = _configuration["Api:Url"] + _configuration["Api:Version"] + _configuration["Api:Check"];

            //Формируем клиента
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            using var client = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = delegate { return true; },
            });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            //Получаем данные по запросу
            using var result = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

            //Если статус ответ - Успешно или Некорректный ответ, возвращаем успешный результат
            if (result?.StatusCode == HttpStatusCode.OK) return true;

            //Если пришёл статус - Неавторизован, возвращаем исключение об этом
            if (result?.StatusCode == HttpStatusCode.Unauthorized) throw new InnerException(Errors.IncorrectToken);

            //Иначе возвращаем общее исключение
            throw new Exception(Errors.ServerError);
        }
        catch (InnerException)
        {
            throw;
        }
        catch (Exception)
        {
            //Возвращаем общее исключение
            throw new Exception(Errors.NoConnection);
        }
    }

    /// <summary>
    /// Метод проверки авторизованного пользователя
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<bool> CheckNotAuthorize()
    {
        try
        {
            //Проверяем данные из файла конфигурации
            if (string.IsNullOrWhiteSpace(_configuration["Api:Url"])) throw new InnerException(Errors.EmptyUrl);
            if (string.IsNullOrWhiteSpace(_configuration["Api:Version"])) throw new InnerException(Errors.EmptyVersion);
            if (string.IsNullOrWhiteSpace(_configuration["Api:Check"])) throw new InnerException(Errors.EmptyUrlCheckConnection);

            //Формируем ссылку запроса
            string url = _configuration["Api:Url"] + _configuration["Api:Version"] + _configuration["Api:Check"] + "anonymous";

            //Формируем клиента
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            using var client = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = delegate { return true; },
            });

            //Получаем данные по запросу
            using var result = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

            //Если статус ответ - Успешно или Некорректный ответ, возвращаем успешный результат
            if (result?.StatusCode == HttpStatusCode.OK) return true;

            //Иначе возвращаем общее исключение
            throw new InnerException(Errors.ServerError);
        }
        catch (InnerException)
        {
            throw;
        }
        catch (Exception)
        {
            //Возвращаем общее исключение
            throw new Exception(Errors.NoConnection);
        }
    }
}