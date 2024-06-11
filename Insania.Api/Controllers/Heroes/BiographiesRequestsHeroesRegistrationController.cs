using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Heroes.BiographiesRequestsHeroesRegistration;
using Insania.Models.Heroes.BiographiesRequestsHeroesRegistration;
using Insania.Models.OutCategories.Exceptions;

namespace Insania.Api.Controllers.Heroes;

/// <summary>
/// Контроллер биографий заявок на регистрацию персонажей
/// </summary>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="biographiesRequestsHeroesRegistration">Интерфейс работы с биографиями заявок на регистрацию персонажей</param>
[Authorize]
[Route("api/v1/biographiesRequestsHeroesRegistration")]
public class BiographiesRequestsHeroesRegistrationController(ILogger<BiographiesRequestsHeroesRegistrationController> logger, 
    IBiographiesRequestsHeroesRegistration biographiesRequestsHeroesRegistration) : BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы с биографиями заявок на регистрацию персонажей
    /// </summary>
    private readonly IBiographiesRequestsHeroesRegistration _biographiesRequestsHeroesRegistration = biographiesRequestsHeroesRegistration;

    /// <summary>
    /// Метод получения биографии заявки на регистрацию персонажа по уникальному ключу
    /// </summary>
    /// <param name="biographyId">Биография</param>
    /// <param name="requestId">Заявка на регистрацию персонажа</param>
    /// <returns cref="GetBiographyRequestHeroRegistrationResponse">Модель ответа получения биографии завяки на регистрацию персонажа</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    [HttpGet("byUnique")]
    public async Task<IActionResult> GetByUnique([FromQuery] long? biographyId, [FromQuery] long? requestId) =>
        await GetAnswerAsync(async () => { return await _biographiesRequestsHeroesRegistration.GetByUnique(biographyId, requestId); });
}