using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Administrators.Administrators;
using Insania.Models.OutCategories.Base;

namespace Insania.Api.Controllers.Administrators;

/// <summary>
/// Контроллер администраторов
/// </summary>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="administrators">Интерфейс работы с администраторами</param>
[Authorize]
[Route("api/v1/administrators")]
public class AdministratorsController(ILogger<AdministratorsController> logger, IAdministrators administrators) : 
    BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы со статусами заявок на регистрацию персонажей
    /// </summary>
    private readonly IAdministrators _administrators = administrators;

    /// <summary>
    /// Метод получения списка администраторов
    /// </summary>
    /// <returns cref="BaseResponseList">Базовая модель ответа для списка</returns>
    [HttpGet("list")]
    public async Task<IActionResult> GetList() => await GetAnswerAsync(_administrators.GetList);
}