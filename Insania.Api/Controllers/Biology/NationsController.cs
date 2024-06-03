using Microsoft.AspNetCore.Mvc;

using Insania.BusinessLogic.Biology.Nations;
using Insania.Api.Controllers.OutCategories;

namespace Insania.Api.Controllers.Biology;

/// <summary>
/// Контроллер наций
/// </summary>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="nations">Интерфейс работы с нациями</param>
[Route("api/v1/nations")]
public class NationsController(ILogger<NationsController> logger, INations nations) : BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы с нациями
    /// </summary>
    private readonly INations _nations = nations;

    /// <summary>
    /// Метод получения списка наций
    /// </summary>
    /// <param name="raceId">Раса</param>
    /// <returns></returns>
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetNationsList([FromQuery] long? raceId) =>
        await GetAnswerAsync(async () => { return await _nations.GetNationsList(raceId); });
}