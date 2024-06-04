using System.Text.Json.Serialization;

namespace Insania.Models.OutCategories.Base;

/// <summary>
/// Базовая модель ответа
/// </summary>
public class BaseResponse
{
    /// <summary>
    /// Признак успешности ответа
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Первичный ключ сущности
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? Id { get; set; }

    /// <summary>
    /// Ошибка
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BaseError? Error { get; set; }

    /// <summary>
    /// Значение
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Value { get; set; }

    /// <summary>
    /// Простой конструктор базовой модели ответа
    /// </summary>
    public BaseResponse()
    {
        
    }

    /// <summary>
    /// Конструктор базовой модели ответа с признаком успешности
    /// </summary>
    /// <param name="success">Признак успешности</param>
    public BaseResponse(bool success): this()
    {
        Success = success;
    }

    /// <summary>
    /// Конструктор базовой модели ответа с id
    /// </summary>
    /// <param name="success">Признак успешности</param>
    /// <param name="id">Первичный ключ сущности</param>
    public BaseResponse(bool success, long id): this(success)
    {
        Id = id;
    }

    /// <summary>
    /// Конструктор базовой модели ответа с ошибкой
    /// </summary>
    /// <param name="success">Признак успешности</param>
    /// <param name="error">Ошибка</param>
    public BaseResponse(bool success, BaseError? error): this(success)
    {
        Error = error;
    }

    /// <summary>
    /// Конструктор базовой модели ответа с id и значением
    /// </summary>
    /// <param name="success">Признак успешности</param>
    /// <param name="id">Первичный ключ сущности</param>
    /// <param name="value">Значение</param>
    public BaseResponse(bool success, long id, string? value) : this(success, id)
    {
        Value = value;
    }
}