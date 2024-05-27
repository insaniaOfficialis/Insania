using Microsoft.IdentityModel.Tokens;

using Insania.Models.Users.Authentication;

namespace Insania.BusinessLogic.Users.Authentication;

/// <summary>
/// Интерфейс сервиса атуентифкации
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Метод аутентификации по логину и паролю
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль</param>
    /// <returns></returns>
    Task<AuthenticationResponse> Login(string? login, string? password);

    /// <summary>
    /// Метод создания токена
    /// </summary>
    /// <param name="login">Логин</param>
    /// <returns></returns>
    string CreateToken(string login);
    
    /// <summary>
    /// Метод генерации ключа
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <returns></returns>
    SymmetricSecurityKey GetSymmetricSecurityKey(string key);
}