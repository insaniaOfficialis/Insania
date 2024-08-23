using Microsoft.AspNetCore.Mvc;

using Insania.Models.OutCategories.Base;
using Insania.Web.Controllers.OutCategories;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.Users.Users;

namespace Insania.Web.Controllers.Users;

/// <summary>
/// Контроллер пользователей
/// </summary>
/// <param name="logger">Интерфейс записи логов</param>
[Route("web/v1/users")]
public class UsersController(ILogger<UsersController> logger) : BaseController(logger)
{
    /// <summary>
    /// Метод получения страницы регистрации пользователей
    /// </summary>
    /// <returns></returns>
    [HttpGet("view")]
    public IActionResult Index()
    {
        //Возвращаем страницу
        return View("/Pages/Users/RegistrationUser/Index.cshtml", new BaseResponse(true));
    }

    /// <summary>
    /// Метод проверки запроса
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
    /// <exception cref="Exception">Обработанное исключение</exception>
    /// <exception cref="InnerException">Необработанное исключение</exception>
    [HttpGet("checkRequest")]
    public IActionResult CheckRequest([FromQuery] string? login, [FromQuery] string? password,
        [FromQuery] string? lastName, [FromQuery] string? firstName, [FromQuery] string? patronymic, [FromQuery] bool? gender,
        [FromQuery] DateTime? birthDate, [FromQuery] string? phoneNumber, [FromQuery] string? email, [FromQuery] string? linkVK)
    {
        try
        {
            //Формируем модель
            AddUserRequest? request = new(login, password, lastName, firstName, patronymic, gender, birthDate, phoneNumber, email,
                linkVK);

            //Возвращаем результат
            return Ok(request);
        }
        catch(InnerException ex)
        {
            return StatusCode(400, new BaseResponse(false, new BaseError(400, ex.Message)));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new BaseResponse(false, new BaseError(500, ex.Message)));
        }
    }
}