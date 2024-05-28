using Microsoft.AspNetCore.Mvc;

using Insania.Api.Controllers.OutCategories;
using Insania.BusinessLogic.Users.Users;
using Insania.Models.Users.Users;

namespace Insania.Api.Controllers.Users;

/// <summary>
/// Контроллер аутентификации
/// </summary>
/// <param name="logger">Интерфейс сервиса записи логов</param>
/// <param name="users">Интерфейс работы с пользователями</param>
[Route("api/v1/registration")]
public class RegistrationController(ILogger<RegistrationController> logger, IUsers users) :
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
}