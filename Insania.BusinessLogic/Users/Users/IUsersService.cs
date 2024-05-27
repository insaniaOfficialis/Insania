using Insania.Models.OutCategories.Base;
using Insania.Models.Users.Users;

namespace Insania.BusinessLogic.Users.Users;

/// <summary>
/// Интерфейс сервиса работы с пользователем
/// </summary>
public interface IUsersService
{
    /// <summary>
    /// Метод добавления пользователя
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <returns></returns>
    Task<BaseResponse> AddUser(AddUserRequest? request);
}