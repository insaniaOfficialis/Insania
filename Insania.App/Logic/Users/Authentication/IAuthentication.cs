using Insania.Models.Users.Authentication;

namespace Insania.App.Logic.Users.Authentication;

/// <summary>
/// Интерфейс сервиса аутентификации
/// </summary>
public interface IAuthentication
{
    /// <summary>
    /// Метод аутентификации по логину и паролю
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль</param>
    /// <returns></returns>
    Task<AuthenticationResponse> Login(string login, string password);
}