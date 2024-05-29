using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Politics.Countries;

namespace Insania.Api.Controllers.Politics;

/// <summary>
/// Контроллер стран
/// </summary>
/// <param name="logger">Интерфейс сервиса записи логов</param>
/// <param name="countries">Интерфейс работы со странами</param>
[Route("api/v1/countries")]
public class CountriesController(ILogger<CountriesController> logger, ICountries countries) : BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы со странами
    /// </summary>
    private readonly ICountries _countries = countries;

    /// <summary>
    /// Метод получения списка стран
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetCountriesList() =>
        await GetAnswerAsync(async () => { return await _countries.GetCountriesList(); });
}