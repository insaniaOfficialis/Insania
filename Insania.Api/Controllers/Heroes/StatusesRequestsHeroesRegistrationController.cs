using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Heroes.StatusesRequestsHeroesRegistration;
using Insania.Models.OutCategories.Base;

namespace Insania.Api.Controllers.Heroes;

/// <summary>
/// Контроллер статусов заявок на регистрацию персонажей
/// </summary>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="statusesRequestsHeroesRegistration">Интерфейс работы со статусами заявок на регистрацию персонажей</param>
[Route("api/v1/statusesRequestsHeroesRegistration")]
public class StatusesRequestsHeroesRegistrationController(ILogger<StatusesRequestsHeroesRegistrationController> logger,
    IStatusesRequestsHeroesRegistration statusesRequestsHeroesRegistration) : BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы со статусами заявок на регистрацию персонажей
    /// </summary>
    private readonly IStatusesRequestsHeroesRegistration _statusesRequestsHeroesRegistration = statusesRequestsHeroesRegistration;

    /// <summary>
    /// Метод получения списка статусов заявок на регистрацию персонажей
    /// </summary>
    /// <returns cref="BaseResponseList">Базовая модель ответа для списка</returns>
    [HttpGet("list")]
    public async Task<IActionResult> GetList() => await GetAnswerAsync(_statusesRequestsHeroesRegistration.GetList);
}