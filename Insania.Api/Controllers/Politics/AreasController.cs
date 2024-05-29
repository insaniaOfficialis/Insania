using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Politics.Areas;

namespace Insania.Api.Controllers.Politics;

/// <summary>
/// Контроллер областей
/// </summary>
/// <param name="logger">Интерфейс сервиса записи логов</param>
/// <param name="areas">Интерфейс работы с областями</param>
[Route("api/v1/areas")]
public class AreasController(ILogger<AreasController> logger, IAreas areas) : BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы с областями
    /// </summary>
    private readonly IAreas _areas = areas;

    /// <summary>
    /// Метод получения списка областей
    /// </summary>
    /// <param name="regionId">Регион</param>
    /// <param name="ownershipId">Владение</param>
    /// <returns></returns>
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetAreasList([FromQuery] long? regionId, [FromQuery] long? ownershipId) =>
        await GetAnswerAsync(async () => { return await _areas.GetAreasList(regionId, ownershipId); });
}