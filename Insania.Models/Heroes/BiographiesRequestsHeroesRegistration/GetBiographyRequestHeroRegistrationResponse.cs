using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.Models.Heroes.BiographiesRequestsHeroesRegistration;

/// <summary>
/// Модель ответа получения биографии завяки на регистрацию персонажа
/// </summary>
public class GetBiographyRequestHeroRegistrationResponse : BaseResponse
{
    /// <summary>
    /// Ссылка на заявку
    /// </summary>
    public long? RequestId { get; set; }

    /// <summary>
    /// Ссылка на биографию персонажа
    /// </summary>
    public long? BiographyId { get; set; }

    /// <summary>
    /// Решение
    /// </summary>
    public bool? Decision { get; set; }

    /// <summary>
    /// Комментарий
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Простой конструктор модели ответа получения биографии завяки на регистрацию персонажа
    /// </summary>
    public GetBiographyRequestHeroRegistrationResponse() : base()
    {

    }

    /// <summary>
    /// Конструктор модели ответа получения биографии завяки на регистрацию персонажа
    /// </summary>
    /// <param name="success">Признак успешности</param>
    /// <param name="id">Первичный ключ сущности</param>
    /// <param name="requestId">Заявка на регистрацию персонажа</param>
    /// <param name="biographyId">Биография</param>
    /// <param name="decision">Решение</param>
    /// <param name="comment">Комментарий</param>
    /// <exception cref="Exception">Необработанное исключение</exception>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    public GetBiographyRequestHeroRegistrationResponse(bool success, long id, long? requestId, long? biographyId, bool? decision,
        string? comment) : base(success, id)
    {
        //Проверяем входные данные
        if (id == 0) throw new InnerException(Errors.EmptyId);
        if ((requestId ?? 0) == 0) throw new InnerException(Errors.EmptyRequestHeroRegistration);
        if ((biographyId ?? 0) == 0) throw new InnerException(Errors.EmptyBiography);
        if (decision == false && string.IsNullOrWhiteSpace(comment)) throw new InnerException(Errors.EmtryCommentOnDecision);

        //Заполняем модель
        RequestId = requestId;
        BiographyId = biographyId;
        Decision = decision;
        Comment = comment;
    }
}