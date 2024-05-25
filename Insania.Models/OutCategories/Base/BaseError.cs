namespace Insania.Models.OutCategories.Base;

/// <summary>
/// Базовая модель ошибки
/// </summary>
public class BaseError
{
    /// <summary>
    /// Код ошибки
    /// </summary>
    public int? Code { get; set; }

    /// <summary>
    /// Текст ошибки
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Простой конструктор базовой модели ошибки
    /// </summary>
    public BaseError()
    {
        
    }

    /// <summary>
    /// Конструктор базовой модели ошибки с кодом и текстом ошибки
    /// </summary>
    /// <param name="code">Код ошибки</param>
    /// <param name="message">Текст ошибки</param>
    public BaseError(int code, string message): this()
    {
        Code = code;
        Message = message;
    }
}