using Insania.Models.OutCategories.Base;

namespace Insania.Models.Users.Authentication;

/// <summary>
/// Модель ответа аутентификации
/// </summary>
public class AuthenticationResponse : BaseResponse
{
    /// <summary>
    /// Токен доступа
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Простой конструктор модели ответа аутентификации
    /// </summary>
    public AuthenticationResponse() : base()
    {
        
    }

    /// <summary>
    /// Конструктор модели ответа аутентификации с ошибкой
    /// </summary>
    /// <param name="success">Признак успешности</param>
    /// <param name="error">Ошибка</param>
    public AuthenticationResponse(bool success, BaseError? error) : base(success, error)
    {

    }

    /// <summary>
    /// Конструктор модели ответа аутентификации с токеном доступа
    /// </summary>
    /// <param name="success">Признак успешности</param>
    /// <param name="error">Ошибка</param>
    /// <param name="token">Токен доступа</param>
    public AuthenticationResponse(bool success, BaseError? error, string? token) : base(success, error)
    {
        Token = token;
    }
}