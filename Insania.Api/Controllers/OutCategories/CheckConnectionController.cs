using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insania.Api.Controllers.OutCategories;

/// <summary>
/// Контроллер проверки соединения
/// </summary>
/// <param name="logger">Интерфейс записи логов</param>
[Authorize]
[Route("api/v1/check")]
public class CheckConnectionController(ILogger<CheckConnectionController> logger) : BaseController(logger)
{
    /// <summary>
    /// Метод проверки соединения авторизованного пользователя
    /// </summary>
    /// <returns></returns>
    [HttpHead]
    public IActionResult CheckAuthorize()
    {
        return Ok();
    }

    /// <summary>
    /// Метод проверки соединения неавторизованного пользователя
    /// </summary>
    /// <returns></returns>
    [HttpHead]
    [AllowAnonymous]
    [Route("anonymous")]
    public IActionResult CheckNotAuthorize()
    {
        return Ok();
    }
}