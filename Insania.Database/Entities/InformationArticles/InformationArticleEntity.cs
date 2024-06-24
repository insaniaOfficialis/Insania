using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using BaseEntity = Insania.Entities.OutCategories.Base;

namespace Insania.Database.Entities.InformationArticles;

/// <summary>
/// Модель сущности информационной статьи сущности
/// </summary>
public abstract class InformationArticleEntity : BaseEntity
{
    /// <summary>
    /// Ссылка на информационную статью
    /// </summary>
    [Column("information_article_id")]
    [Comment("Ссылка на информационную статью")]
    public long InformationArticleId { get; private set; }

    /// <summary>
    /// Навигационное свойство информационной статьи
    /// </summary>
    public HeaderInformationArticle InformationArticle { get; private set; }

    /// <summary>
    /// Порядковый номер
    /// </summary>
    [Column("sequence_number")]
    [Comment("Порядковый номер")]
    public int? SequenceNumber { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности информационной статьи сущности
    /// </summary>
    public InformationArticleEntity() : base()
    {
        InformationArticle = new();
    }

    /// <summary>
    /// Конструктор модели сущности информационной статьи сущности без id и порядкового номера
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="informationArticle">Ссылка на информационную статью</param>
    public InformationArticleEntity(string user, HeaderInformationArticle informationArticle) : base(user)
    {
        InformationArticleId = informationArticle.Id;
        InformationArticle = informationArticle;
    }

    /// <summary>
    /// Конструктор модели сущности информационной статьи сущности с id и без порядкового номера
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="informationArticle">Ссылка на информационную статью</param>
    public InformationArticleEntity(long id, string user, HeaderInformationArticle informationArticle) : base(id, user)
    {
        InformationArticleId = informationArticle.Id;
        InformationArticle = informationArticle;
    }

    /// <summary>
    /// Конструктор модели сущности информационной статьи сущности без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="informationArticle">Ссылка на информационную статью</param>
    /// <param name="sequenceNumber">орядковый номер</param>
    public InformationArticleEntity(string user, HeaderInformationArticle informationArticle, int sequenceNumber) : base(user)
    {
        InformationArticleId = informationArticle.Id;
        InformationArticle = informationArticle;
        SequenceNumber = sequenceNumber;
    }

    /// <summary>
    /// Конструктор модели сущности информационной статьи сущности с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="informationArticle">Ссылка на информационную статью</param>
    /// <param name="sequenceNumber">орядковый номер</param>
    public InformationArticleEntity(long id, string user, HeaderInformationArticle informationArticle, int sequenceNumber) : base(id, user)
    {
        InformationArticleId = informationArticle.Id;
        InformationArticle = informationArticle;
        SequenceNumber = sequenceNumber;
    }

    /// <summary>
    /// Метод записи информационной статьи
    /// </summary>
    /// <param name="informationArticle">Ссылка на информационную статью</param>
    public void SetInformationArticle(HeaderInformationArticle informationArticle)
    {
        InformationArticleId = informationArticle.Id;
        InformationArticle = informationArticle;
    }

    /// <summary>
    /// Метод записи порядкового номера
    /// </summary>
    /// <param name="sequenceNumber">Порядковый номер</param>
    public void SetSequenceNumber(int sequenceNumber)
    {
        SequenceNumber = sequenceNumber;
    }
}