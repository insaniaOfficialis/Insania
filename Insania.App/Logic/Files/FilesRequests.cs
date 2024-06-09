using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

using Microsoft.Extensions.Configuration;

using Insania.BusinessLogic.Files.Files;
using Insania.Models.Files.Files;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Models.Users.Authentication;
using System.Net.Security;

namespace Insania.App.Logic.Files;

/// <summary>
/// Запросы работы с файлами
/// </summary>
/// <param name="configuration">Интерфейс конфигурации</param>
public class FilesRequests(IConfiguration configuration) : IFiles
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
    /// Метод добавления файла
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <returns></returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<BaseResponse> Add(AddFileRequest? request)
    {
        try
        {
            //Проверяем данные из файла конфигурации
            if (string.IsNullOrWhiteSpace(_configuration["Api:Url"])) throw new InnerException(Errors.EmptyUrl);
            if (string.IsNullOrWhiteSpace(_configuration["Api:Version"])) throw new InnerException(Errors.EmptyVersion);
            if (string.IsNullOrWhiteSpace(_configuration["Api:Files"])) throw new InnerException(Errors.EmptyUrlFiles);

            //Проверяем входные данные
            if (request == null) throw new InnerException(Errors.EmptyRequest);
            if (string.IsNullOrWhiteSpace(request.Type)) throw new InnerException(Errors.EmptyTypeFile);
            if ((request.Id ?? 0) == 0) throw new InnerException(Errors.EmptyEntityFile);
            if (request.Stream == null) throw new InnerException(Errors.EmptyStreamFile);
            if (string.IsNullOrWhiteSpace(request.Name)) throw new InnerException(Errors.EmptyNameFile);

            //Формируем ссылку запроса
            string url = _configuration["Api:Url"] + _configuration["Api:Version"] + _configuration["Api:Files"] + $"{request.Type}/{request.Id}";

            //Формируем клиента
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            using var client = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = delegate { return true; },
            });
            using var multipartFormContent = new MultipartFormDataContent();
            var fileStreamContent = new StreamContent(request.Stream);
            var extention = request.Name[(request.Name.LastIndexOf('.') + 1)..];
            var contentType = "image/" + extention;
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            multipartFormContent.Add(fileStreamContent, name: "file", fileName: request.Name);

            //Получаем данные по запросу
            using var result = await client.PostAsync(url, multipartFormContent) ?? throw new InnerException(Errors.EmptyResponse);

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