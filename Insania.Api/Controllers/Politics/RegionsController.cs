using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Politics.Regions;

namespace Insania.Api.Controllers.Politics;

/// <summary>
/// Контроллер регионов
/// </summary>
/// <param name="logger">Интерфейс сервиса записи логов</param>
/// <param name="regions">Интерфейс работы с регионами</param>
[Route("api/v1/regions")]
public class RegionsController(ILogger<RegionsController> logger, IRegions regions) : BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы с регионами
    /// </summary>
    private readonly IRegions _regions = regions;

    /// <summary>
    /// Метод получения списка регионов
    /// </summary>
    /// <param name="countryId">Страна</param>
    /// <returns></returns>
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetRegionsList([FromQuery] long? countryId) =>
        await GetAnswerAsync(async () => { return await _regions.GetRegionsList(countryId); });
}