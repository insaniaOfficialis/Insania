using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Users.Users;
using Insania.Models.Users.Users;

namespace Insania.Api.Controllers.Users;

/// <summary>
/// Контроллер пользователей
/// </summary>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="users">Интерфейс работы с пользователями</param>
[Route("api/v1/users")]
public class UsersController(ILogger<UsersController> logger, IUsers users) :
    BaseController(logger)
{
    /// <summary>
    /// Интерфейс работы с пользователями
    /// </summary>
    private readonly IUsers _users = users;

    /// <summary>
    /// Метод добавления пользователя
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] AddUserRequest? request) => 
        await GetAnswerAsync(async () => { return await _users.AddUser(request); });

    /// <summary>
    /// Метод проверки доступности логина
    /// </summary>
    /// <param name="login">Логин</param>
    /// <returns></returns>
    [HttpGet]
    [Route("checkLogin")]
    public async Task<IActionResult> CheckLogin([FromQuery] string? login) =>
        await GetAnswerAsync(async () => { return await _users.CheckLogin(login); });
}