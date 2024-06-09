using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Files.Files;
using Insania.Models.Files.Files;

namespace Insania.Api.Controllers.Files;

/// <summary>
/// Контроллер файлов
/// </summary>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="files">Интерфейс работы с файлами</param>
[Route("api/v1/files")]
public class FilesController(ILogger<FilesController> logger, IFiles files) : BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы с файлами
    /// </summary>
    private readonly IFiles _files = files;

    /// <summary>
    /// Метод добавления файла
    /// </summary>
    /// <param name="type">Тип</param>
    /// <param name="id">Первичный ключ</param>
    /// <param name="file">Файл</param>
    /// <returns></returns>
    [HttpPost("{type}/{id}")]
    public async Task<IActionResult> Add([FromRoute] string type, [FromRoute] long id, IFormFile file) =>
        await GetAnswerAsync(async () => 
        {
            AddFileRequest request = new(id, file.FileName, type, file.OpenReadStream(), 0);

            return await _files.Add(request); 
        });

    /// <summary>
    /// Метод получения файла по первичному ключу
    /// </summary>
    /// <param name="id">Первичный ключ</param>
    /// <returns cref="FileStreamResult">Файл</returns>
    [HttpGet("{id}")]
    public async Task<FileStreamResult?> GetById([FromRoute] long id)
    {
        try
        {
            //Получаем данные по файлу
            var response = await _files.GetById(id);

            //Проверяем данные по файлу
            if (response == null || !response.Success || string.IsNullOrWhiteSpace(response.ContentType) || response.FileStream == null) return null;

            //Возвращаем ответ
            return File(response.FileStream, response.ContentType);
        }
        catch
        {
            return null;
        }
    }
}