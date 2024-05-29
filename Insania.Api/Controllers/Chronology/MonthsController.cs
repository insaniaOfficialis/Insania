using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Chronology.Months;

namespace Insania.Api.Controllers.Chronology;

/// <summary>
/// Контроллер месяцев
/// </summary>
/// <param name="logger">Интерфейс сервиса записи логов</param>
/// <param name="months">Интерфейс работы с месяцами</param>
[Route("api/v1/months")]
public class MonthsController(ILogger<MonthsController> logger, IMonths months) : BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы с месяцами
    /// </summary>
    private readonly IMonths _months = months;

    /// <summary>
    /// Метод получения списка месяцев
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetMonthsList() =>
        await GetAnswerAsync(async () => { return await _months.GetMonthsList(); });
}