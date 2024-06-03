using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Appearance.EyesColors;

namespace Insania.Api.Controllers.Appearance;

/// <summary>
/// Контроллер цветов глаз
/// </summary>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="eyesColors">Интерфейс работы с цветами глаз</param>
[Route("api/v1/eyesColors")]
public class EyesColorsController(ILogger<EyesColorsController> logger, IEyesColors eyesColors) : BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы с цветами глаз
    /// </summary>
    private readonly IEyesColors _eyesColors = eyesColors;

    /// <summary>
    /// Метод получения списка цветов глаз
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetEyesColorsList() => await GetAnswerAsync(_eyesColors.GetEyesColorsList);
}