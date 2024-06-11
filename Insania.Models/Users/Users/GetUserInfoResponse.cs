using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.Models.Users.Users;

/// <summary>
/// Модель ответа получения информации о пользователе
/// </summary>
public class GetUserInfoResponse : BaseResponse
{
    /// <summary>
    /// Логин
    /// </summary>
    public string? Login { get; set; }

    /// <summary>
    /// Полное имя
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Инициалы
    /// </summary>
    public string? Initials { get; set; }

    /// <summary>
    /// Признак заблокированного пользователя
    /// </summary>
    public bool? IsBlocked { get; set; }

    /// <summary>
    /// Пол (истина - мужской/ложь - женский)
    /// </summary>
    public bool? Gender { get; set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Номер телфона
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Почта
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Ссылка в вк
    /// </summary>
    public string? LinkVK { get; set; }

    /// <summary>
    /// Роли пользователя
    /// </summary>
    public string[]? Roles { get; set; }

    /// <summary>
    /// Простой конструктор модели ответа получения информации о пользователе
    /// </summary>
    public GetUserInfoResponse() : base()
    {

    }

    /// <summary>
    /// Конструктор модели ответа получения информации о пользователе
    /// </summary>
    /// <param name="success">Признак успешности</param>
    /// <param name="id">Первичный ключ сущности</param>
    /// <param name="requestId">Заявка на регистрацию персонажа</param>
    /// <param name="biographyId">Биография</param>
    /// <param name="decision">Решение</param>
    /// <param name="comment">Комментарий</param>
    /// <exception cref="Exception">Необработанное исключение</exception>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    public GetUserInfoResponse(bool success, long id, long? requestId, long? biographyId, bool? decision,
        string? comment) : base(success, id)
    {
        //Проверяем входные данные
        if (id == 0) throw new InnerException(Errors.EmptyId);
        if ((requestId ?? 0) == 0) throw new InnerException(Errors.EmptyRequestHeroRegistration);
        if ((biographyId ?? 0) == 0) throw new InnerException(Errors.EmptyBiography);
        if (decision == false && string.IsNullOrWhiteSpace(comment)) throw new InnerException(Errors.EmtryCommentOnDecision);

        //Заполняем модель
    }
}