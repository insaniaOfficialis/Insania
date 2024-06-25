using Microsoft.AspNetCore.Mvc;

namespace Insania.Web.Controllers.Users;

/// <summary>
/// Контроллер пользователей
/// </summary>
[Route("web/v1/user")]
public class UsersController : Controller
{
    /// <summary>
    /// Метод получения страницы регистрации пользователей
    /// </summary>
    /// <returns></returns>
    [Route("view")]
    public IActionResult Index()
    {
        //Возвращаем страницу
        return View("/Pages/Users/RegistrationUser/Index.cshtml");
    }
}