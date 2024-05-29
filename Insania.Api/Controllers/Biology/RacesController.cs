using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Biology.Races;

namespace Insania.Api.Controllers.Biology;

/// <summary>
/// Контроллер рас
/// </summary>
/// <param name="logger">Интерфейс сервиса записи логов</param>
/// <param name="races">Интерфейс работы с расами</param>
[Route("api/v1/races")]
public class RacesController(ILogger<RacesController> logger, IRaces races) : BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы с расами
    /// </summary>
    private readonly IRaces _races = races;

    /// <summary>
    /// Метод получения списка рас
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetRacesList() =>
        await GetAnswerAsync(async () => { return await _races.GetRacesList(); });
}