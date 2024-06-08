using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.Models.Heroes.RequestsHeroesRegistration;

/// <summary>
/// Модель ответа получения заявки на регистрацию персонажа
/// </summary>
public class GetRequestRegistrationHeroResponse : BaseResponse
{
    /// <summary>
    /// Ссылка на персонажа
    /// </summary>
    public long? HeroId { get; set; }

    /// <summary>
    /// Ссылка на статуса
    /// </summary>
    public long? StatusId { get; set; }

    /// <summary>
    /// Ссылка на ответственного администратора
    /// </summary>
    public long? AdministratorId { get; set; }

    /// <summary>
    /// Решение по блоку общее
    /// </summary>
    public bool? GeneralBlockDecision { get; set; }

    /// <summary>
    /// Комментарий к блоку общее
    /// </summary>
    public string? CommentOnGeneralBlock { get; set; }

    /// <summary>
    /// Решение по блоку дата рождения
    /// </summary>
    public bool? BirthDateBlockDecision { get; set; }

    /// <summary>
    /// Комментарий к блоку дата рождения
    /// </summary>
    public string? CommentOnBirthDateBlock { get; set; }

    /// <summary>
    /// Решение по блоку местоположение
    /// </summary>
    public bool? LocationBlockDecision { get; set; }

    /// <summary>
    /// Комментарий к блоку местоположение
    /// </summary>
    public string? CommentOnLocationBlock { get; set; }

    /// <summary>
    /// Решение по блоку внешность
    /// </summary>
    public bool? AppearanceBlockDecision { get; set; }

    /// <summary>
    /// Комментарий к блоку внешность
    /// </summary>
    public string? CommentOnAppearanceBlock { get; set; }

    /// <summary>
    /// Решение по блоку изображение
    /// </summary>
    public bool? ImageBlockDecision { get; set; }

    /// <summary>
    /// Комментарий к блоку изображение
    /// </summary>
    public string? CommentOnImageBlock { get; set; }

    /// <summary>
    /// Простой конструктор модели ответа получения заявки на регистрацию персонажа
    /// </summary>
    public GetRequestRegistrationHeroResponse() : base()
    {

    }

    /// <summary>
    /// Конструктор модели ответа получения заявки на регистрацию персонажа
    /// </summary>
    /// <param name="success">Признак успешности</param>
    /// <param name="id">Первичный ключ сущности</param>
    /// <param name="heroId">Персонаж</param>
    /// <param name="statusId">Статус</param>
    /// <param name="administratorId">Ответсвенный</param>
    /// <param name="generalBlockDecision">Решение по блоку общее</param>
    /// <param name="commentOnGeneralBlock">Комментарий к блоку общее</param>
    /// <param name="birtDateBlockDecision">Решение по блоку дата рождения</param>
    /// <param name="commentOnBirthDateBlock">Комментарий к блоку дата рождения</param>
    /// <param name="locationBlockDecision">Решение по блоку местоположение</param>
    /// <param name="commentOnLocationBlock">Комментарий к блоку местоположение</param>
    /// <param name="appearanceBlockDecision">Решение по блоку внешности</param>
    /// <param name="commentOnAppearenceBlock">Комментарий к блоку внешности</param>
    /// <param name="imageBlockDecision">Решение по блоку изображения</param>
    /// <param name="commentOnImageBlock">Комментарий к блоку изображения</param>
    /// <exception cref="Exception">Необработанное исключение</exception>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    public GetRequestRegistrationHeroResponse(bool success, long id, long? heroId, long? statusId, long? administratorId,
        bool? generalBlockDecision, string? commentOnGeneralBlock, bool? birtDateBlockDecision, string? commentOnBirthDateBlock,
        bool? locationBlockDecision, string? commentOnLocationBlock, bool? appearanceBlockDecision,
        string? commentOnAppearenceBlock, bool? imageBlockDecision, string? commentOnImageBlock) : base(success, id)
    {
        //Проверяем входные данные
        if (id == 0) throw new InnerException(Errors.EmptyId);
        if ((heroId ?? 0) == 0) throw new InnerException(Errors.EmptyHero);
        if ((statusId ?? 0) == 0) throw new InnerException(Errors.EmptyStatusRequestsHeroesRegistration);
        if ((administratorId ?? 0) == 0) throw new InnerException(Errors.EmptyAdministrator);
        if (generalBlockDecision == false && string.IsNullOrWhiteSpace(commentOnGeneralBlock)) throw new InnerException(Errors.EmtryCommentOnDecision);
        if (birtDateBlockDecision == false && string.IsNullOrWhiteSpace(commentOnBirthDateBlock)) throw new InnerException(Errors.EmtryCommentOnDecision);
        if (locationBlockDecision == false && string.IsNullOrWhiteSpace(commentOnLocationBlock)) throw new InnerException(Errors.EmtryCommentOnDecision);
        if (appearanceBlockDecision == false && string.IsNullOrWhiteSpace(commentOnAppearenceBlock)) throw new InnerException(Errors.EmtryCommentOnDecision);
        if (imageBlockDecision == false && string.IsNullOrWhiteSpace(commentOnImageBlock)) throw new InnerException(Errors.EmtryCommentOnDecision);

        //Заполняем модель
        HeroId = heroId;
        StatusId = statusId;
        AdministratorId = administratorId;
        GeneralBlockDecision = generalBlockDecision;
        CommentOnGeneralBlock = commentOnGeneralBlock;
        BirthDateBlockDecision = birtDateBlockDecision;
        CommentOnBirthDateBlock = commentOnBirthDateBlock;
        LocationBlockDecision = locationBlockDecision;
        CommentOnLocationBlock = commentOnLocationBlock;
        AppearanceBlockDecision = appearanceBlockDecision;
        CommentOnAppearanceBlock = commentOnAppearenceBlock;
        ImageBlockDecision = imageBlockDecision;
        CommentOnImageBlock = commentOnImageBlock;
    }
}