using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Heroes.BiographiesHeroes;
using Insania.Models.Heroes.BiographiesHeroes;

namespace Insania.Api.Controllers.Heroes;

/// <summary>
/// Контроллер биографий персонажей
/// </summary>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="biographiesHeroes">Интерфейс работы с биографиями персонажей</param>
[Authorize]
[Route("api/v1/biographiesHeroes")]
public class BiographiesHeroesController(ILogger<BiographiesHeroesController> logger, IBiographiesHeroes biographiesHeroes) : 
    BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы с биографиями персонажей
    /// </summary>
    private readonly IBiographiesHeroes _biographiesHeroes = biographiesHeroes;

    /// <summary>
    /// Метод получения биографий персонажа
    /// </summary>
    /// <param name="heroId">Персонаж</param>
    /// <returns cref="GetBiographiesHeroResponseList">Модель ответа получения биографий персонажа</returns>
    [HttpGet("list")]
    public async Task<IActionResult> GetList([FromQuery] long? heroId) =>
        await GetAnswerAsync(async () => { return await _biographiesHeroes.GetList(heroId); });
}