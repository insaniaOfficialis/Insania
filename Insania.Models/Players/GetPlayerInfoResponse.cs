using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.Models.Players;

/// <summary>
/// Модель ответа получения информации об игроке
/// </summary>
public class GetPlayerInfoResponse : BaseResponse
{
    /// <summary>
    /// Баллы верности
    /// </summary>
    public int? LoyaltyPoints { get; set; }

    /// <summary>
    /// Простой конструктор модели ответа получения информации о пользователе
    /// </summary>
    public GetPlayerInfoResponse() : base()
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
    public GetPlayerInfoResponse(bool success, long id, long? requestId, long? biographyId, bool? decision,
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
