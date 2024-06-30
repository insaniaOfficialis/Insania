using Microsoft.AspNetCore.Mvc;

using Insania.Web.Controllers.OutCategories;
using Insania.Models.Users.Users;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;

namespace Insania.Web.Controllers.Heroes;

/// <summary>
/// Контроллер персонажей
/// </summary>
/// <param name="logger">Интерфейс записи логов</param>
[Route("web/v1/heroes")]
public class HeroesController(ILogger<HeroesController> logger) : BaseController(logger)
{
    /// <summary>
    /// Метод получения страницы регистрации персонажа
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль</param>
    /// <param name="lastName">Фамилия</param>
    /// <param name="firstName">Имя</param>
    /// <param name="patronymic">Отчество</param>
    /// <param name="gender">Пол (истина - мужской/ложь - женский)</param>
    /// <param name="birthDate">Дата рождения</param>
    /// <param name="phoneNumber">Номер телефона</param>
    /// <param name="email">Почта</param>
    /// <param name="linkVK">Ссылка в вк</param>
    /// <returns></returns>
    [HttpGet("view")]
    public IActionResult Index([FromQuery] string? login, [FromQuery] string? password,
        [FromQuery] string? lastName, [FromQuery] string? firstName, [FromQuery] string? patronymic, [FromQuery] bool? gender,
        [FromQuery] DateTime? birthDate, [FromQuery] string? phoneNumber, [FromQuery] string? email, [FromQuery] string? linkVK)
    {
        try
        {
            //Формируем модель
            AddUserRequest? request = new(login, password, lastName, firstName, patronymic, gender, birthDate, phoneNumber, email,
                linkVK);

            //Возвращаем страницу
            return View("/Pages/Heroes/RegistrationHero/Index.cshtml", request);
        }
        catch (InnerException ex)
        {
            return StatusCode(400, new BaseResponse(false, new BaseError(400, ex.Message)));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new BaseResponse(false, new BaseError(500, ex.Message)));
        }
    }
}