﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Heroes.RequestsHeroesRegistration;
using Insania.Models.Heroes.RequestsHeroesRegistration;
using Insania.Models.OutCategories.Exceptions;

namespace Insania.Api.Controllers.Heroes;

/// <summary>
/// Контроллер заявок на регистрацию персонажей
/// </summary>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="requestsHeroesRegistration">Интерфейс работы с заявками на регистрацию персонажей</param>
[Authorize]
[Route("api/v1/requestsHeroesRegistration")]
public class RequestsHeroesRegistrationController(ILogger<RequestsHeroesRegistrationController> logger, 
    IRequestsHeroesRegistration requestsHeroesRegistration) : BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы с заявками на регистрацию персонажей
    /// </summary>
    private readonly IRequestsHeroesRegistration _requestsHeroesRegistration = requestsHeroesRegistration;

    /// <summary>
    /// Метод получения заявки на регистрацию персонажа
    /// </summary>
    /// <param name="id">Первичный ключ заявки</param>
    /// <returns cref="GetRequestRegistrationHeroResponse">Ответ</returns>
    [HttpGet("byId")]
    public async Task<IActionResult> GetById([FromQuery] long? id) =>
        await GetAnswerAsync(async () => { return await _requestsHeroesRegistration.GetById(id); });

    /// <summary>
    /// Метод получения заявки на регистрацию персонажа по персонажу
    /// </summary>
    /// <param name="heroId">Персонаж</param>
    /// <returns cref="GetRequestRegistrationHeroResponse">Ответ</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    [HttpGet("byHero")]
    public async Task<IActionResult> GetByHero([FromQuery] long? heroId) =>
        await GetAnswerAsync(async () => { return await _requestsHeroesRegistration.GetByHero(heroId); });
}