using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Heroes.Heroes;
using Insania.Models.Heroes.Heroes;

namespace Insania.Api.Controllers.Heroes;

/// <summary>
/// Контроллер персонажей
/// </summary>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="heroes">Интерфейс работы с персонажами</param>
[Route("api/v1/heroes")]
public class HeroesController(ILogger<HeroesController> logger, IHeroes heroes) : BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы с персонажами
    /// </summary>
    private readonly IHeroes _heroes = heroes;

    /// <summary>
    /// Метод регистрации персонажа
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <returns></returns>
    [HttpPost("registration")]
    public async Task<IActionResult> Registration([FromBody] AddHeroRequest? request) =>
        await GetAnswerAsync(async () => { return await _heroes.Registration(request); });

    /// <summary>
    /// Метод получения персонажа по первичному ключу
    /// </summary>
    /// <param name="id">Первичный ключ</param>
    /// <returns cref="GetHeroResponse">Модель ответа получения персонажа</returns>
    [Authorize]
    [HttpGet("byId")]
    public async Task<IActionResult> GetById([FromQuery] long? id) => 
        await GetAnswerAsync(async () => { return await _heroes.GetById(id); });
}