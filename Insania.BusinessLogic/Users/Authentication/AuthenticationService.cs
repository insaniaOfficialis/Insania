using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using Insania.Database.Entities.Users;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Models.Users.Authentication;

namespace Insania.BusinessLogic.Users.Authentication;

/// <summary>
/// Сервис атуентифкации
/// </summary>
/// <param name="userManager">Менеджер пользователей</param>
/// <param name="userManager">Интерфейс конфигурации</param>
public class AuthenticationService(UserManager<User> userManager, IConfiguration configuration) : IAuthenticationService
{
    /// <summary>
    /// Менеджер пользователей
    /// </summary>
    private readonly UserManager<User> _userManager = userManager;

    /// <summary>
    /// Интерфейс конфигурации
    /// </summary>
    private readonly IConfiguration _configuration = configuration;

    /// <summary>
    /// Метод аутентификации по логину и паролю
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль</param>
    /// <returns></returns>
    public async Task<AuthenticationResponse> Login(string? login, string? password)
    {
        try
        {
            //Проверяем, что передали логин
            if (string.IsNullOrWhiteSpace(login)) throw new InnerException(Errors.EmptyLogin);

            //Проверяем, что передали пароль
            if (String.IsNullOrWhiteSpace(password)) throw new InnerException(Errors.EmptyPassword);

            //Проверяем наличие пользователя
            var user = await _userManager.FindByNameAsync(login) ?? throw new InnerException(Errors.NotExistsUser);

            //Проверяем, что пользователь не заблокирован
            if (user.IsBlocked) throw new InnerException(Errors.UserIsBlocked);

            //Проверяем корректность пароля
            PasswordHasher<User> passwordHasher = new();
            var validatePassword = passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);
            if (validatePassword != PasswordVerificationResult.Success) throw new InnerException(Errors.IncorrectPassword);

            //Создаём токен
            var token = CreateToken(login);

            //Возвращаем результат с токеном
            return new AuthenticationResponse(true, null, token);
        }

        //Обрабатываем внутренние исключения
        catch (InnerException ex)
        {
            return new AuthenticationResponse(false, new BaseError(400, ex.Message));
        }
        //Обрабатываем системные исключения
        catch (Exception ex)
        {
            return new AuthenticationResponse(false, new BaseError(500, ex.Message));
        }
    }

    /// <summary>
    /// Метод создания токена
    /// </summary>
    /// <param name="login">Логин</param>
    /// <returns></returns>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public string CreateToken(string login)
    {
        //Проверяем, что передали логин
        if (string.IsNullOrWhiteSpace(login)) throw new InnerException(Errors.EmptyLogin);

        //Получаем параметры генерации токена
        var claims = new List<Claim> { new (ClaimTypes.Name, login) };
        string? issuer = _configuration["TokenOptions:Issuer"] ?? throw new InnerException(Errors.EmptyIssuer);
        string? audience = _configuration["TokenOptions:Audience"] ?? throw new InnerException(Errors.EmptyAudience);
        if (!double.TryParse(_configuration["TokenOptions:Expires"], out var expires)) throw new InnerException(Errors.EmptyExpires);
        string? key = _configuration["TokenOptions:Key"] ?? throw new InnerException(Errors.EmptyKeyToken);

        //Создаем JWT-токен
        var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(expires),
                signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

        //Возвращаем созданный токен
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    /// <summary>
    /// Метод генерации ключа
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <returns></returns>
    public SymmetricSecurityKey GetSymmetricSecurityKey(string key) => new(Encoding.ASCII.GetBytes(key));
}