using Microsoft.AspNetCore.Mvc;

using Insania.BusinessLogic.Sociology.PrefixesNames;
using Insania.Api.Controllers.OutCategories;

namespace Insania.Api.Controllers.Sociology;

/// <summary>
/// Контроллер префиксов имён
/// </summary>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="prefixesNames">Интерфейс работы с префиксами имён</param>
[Route("api/v1/prefixesNames")]
public class PrefixesNamesController(ILogger<PrefixesNamesController> logger, IPrefixesNames prefixesNames) : BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы с префиксами имён
    /// </summary>
    private readonly IPrefixesNames _prefixesNames = prefixesNames;

    /// <summary>
    /// Метод получения списка префиксов имён
    /// </summary>
    /// <param name="nationId">Нация</param>
    /// <returns></returns>
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetList([FromQuery] long? nationId) =>
        await GetAnswerAsync(async () => { return await _prefixesNames.GetList(nationId); });
}