using Insania.Models.OutCategories.Base;
using Insania.Models.Users.Users;

namespace Insania.BusinessLogic.Users.Users;

/// <summary>
/// Интерфейс работы с пользователями
/// </summary>
public interface IUsers
{
    /// <summary>
    /// Метод добавления пользователя
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <returns></returns>
    Task<BaseResponse> AddUser(AddUserRequest? request);

    /// <summary>
    /// Метод проверки доступности логина
    /// </summary>
    /// <param name="login">Логин</param>
    /// <returns></returns>
    Task<BaseResponse> CheckLogin(string? login);
}