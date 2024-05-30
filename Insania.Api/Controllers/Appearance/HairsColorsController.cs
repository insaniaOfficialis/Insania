using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Appearance.HairsColors;

namespace Insania.Api.Controllers.Appearance;

/// <summary>
/// Контроллер цветов лиц
/// </summary>
/// <param name="logger">Интерфейс сервиса записи логов</param>
/// <param name="hairsColors">Интерфейс работы с цветами лиц</param>
[Route("api/v1/hairsColors")]
public class HairsColorsController(ILogger<HairsColorsController> logger, IHairsColors hairsColors) : BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы с цветами лиц
    /// </summary>
    private readonly IHairsColors _hairsColors = hairsColors;

    /// <summary>
    /// Метод получения списка цветов лиц
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetHairsColorsList() => await GetAnswerAsync(_hairsColors.GetHairsColorsList);
}