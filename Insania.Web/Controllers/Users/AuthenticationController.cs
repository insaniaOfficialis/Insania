using Microsoft.AspNetCore.Mvc;

namespace Insania.Web.Controllers.Users;

/// <summary>
/// Контроллер аутентификации
/// </summary>
[Route("web/v1/authentication")]
public class AuthenticationController : Controller
{
    /// <summary>
    /// Метод получения страницы аутентификации
    /// </summary>
    /// <returns></returns>
    [Route("view")]
    public IActionResult Index()
    {
        //Возвращаем страницу
        return View("/Pages/Users/Authentication/Index.cshtml");
    }
}