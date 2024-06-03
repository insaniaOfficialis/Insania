using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Appearance.TypesBodies;

namespace Insania.Api.Controllers.Appearance;

/// <summary>
/// Контроллер типов телосложений
/// </summary>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="typesBodies">Интерфейс работы с типами телосложений</param>
[Route("api/v1/typesBodies")]
public class TypesBodiesController(ILogger<TypesBodiesController> logger, ITypesBodies typesBodies) : BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы с типами телосложений
    /// </summary>
    private readonly ITypesBodies _typesBodies = typesBodies;

    /// <summary>
    /// Метод получения списка типов телосложений
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetTypesBodiesList() => await GetAnswerAsync(_typesBodies.GetTypesBodiesList);
}