using Insania.Models.Users.Authentication;

namespace Insania.BusinessLogic.Users.Authentication;

/// <summary>
/// Интерфейс атуентифкации
/// </summary>
public interface IAuthentication
{
    /// <summary>
    /// Метод аутентификации по логину и паролю
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль</param>
    /// <returns></returns>
    Task<AuthenticationResponse> Login(string? login, string? password);
}