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
    /// <param name="type"></param>
    /// <param name="id"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("{type}/{id}")]
    public async Task<IActionResult> Add([FromRoute] string type, [FromRoute] long id, IFormFile file) =>
        await GetAnswerAsync(async () => 
        {
            AddFileRequest request = new(id, file.FileName, type, file.OpenReadStream(), 0);

            return await _files.Add(request); 
        });
}