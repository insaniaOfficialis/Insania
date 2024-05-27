using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Users.Authentication;

namespace Insania.Api.Controllers.Users;

/// <summary>
/// Контроль аутентификации
/// </summary>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="authentication">Интерфейс сервиса атуентифкации</param>
[Route("api/v1/authentication")]
public class AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authentication) : 
    BaseController(logger)
{
    /// <summary>
    /// Интерфейс атуентифкации
    /// </summary>
    private readonly IAuthenticationService _authentication = authentication;

    /// <summary>
    /// Метод аутентификации по логину и паролю
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль</param>
    /// <returns></returns>
    [HttpGet]
    [Route("login")]
    public async Task<IActionResult> Login([FromQuery] string? login, [FromQuery] string? password) => 
        await GetAnswerAsync(async () => { return await _authentication.Login(login, password); });
}