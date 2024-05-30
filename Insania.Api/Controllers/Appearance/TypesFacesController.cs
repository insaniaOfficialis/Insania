using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Appearance.TypesFaces;

namespace Insania.Api.Controllers.Appearance;

/// <summary>
/// Контроллер типов лиц
/// </summary>
/// <param name="logger">Интерфейс сервиса записи логов</param>
/// <param name="typesFaces">Интерфейс работы с типами лиц</param>
[Route("api/v1/typesFaces")]
public class TypesFacesController(ILogger<TypesFacesController> logger, ITypesFaces typesFaces) : BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы с типами лиц
    /// </summary>
    private readonly ITypesFaces _typesFaces = typesFaces;

    /// <summary>
    /// Метод получения списка типов лиц
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetTypesFacesList() => await GetAnswerAsync(_typesFaces.GetTypesFacesList);
}