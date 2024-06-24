using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.InformationArticles;

/// <summary>
/// Модель сущности радела информационной статьи
/// </summary>
[Table("dir_sections_information_articles")]
[Comment("Разделы информационных статей")]
public class SectionInformationArticle : Guide
{
    /// <summary>
    /// Порядковый номер
    /// </summary>
    [Column("sequence_number")]
    [Comment("Порядковый номер")]
    public int? SequenceNumber { get; private set; }

    /// <summary>
    /// Ссылка на родителя
    /// </summary>
    [Column("parent_id")]
    [Comment("Ссылка на родителя")]
    public long? ParentId { get; private set; }

    /// <summary>
    /// Навигационное свойство родителя
    /// </summary>
    public SectionInformationArticle? Parent { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности радела информационной статьи
    /// </summary>
    public SectionInformationArticle() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности радела информационной статьи без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    /// <param name="sequenceNumber">Порядковый номер</param>
    /// <param name="parent">Родитель</param>
    public SectionInformationArticle(string user, string name, int sequenceNumber, SectionInformationArticle? parent) : base(user,
        name)
    {
        SequenceNumber = sequenceNumber;
        ParentId = parent?.Id;
        Parent = parent;
    }

    /// <summary>
    /// Конструктор модели сущности радела информационной статьи с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    /// <param name="sequenceNumber">Порядковый номер</param>
    /// <param name="parent">Родитель</param>
    public SectionInformationArticle(long id, string user, string name, int sequenceNumber, SectionInformationArticle? parent) : 
        base(id, user, name)
    {
        SequenceNumber = sequenceNumber;
        ParentId = parent?.Id;
        Parent = parent;
    }

    /// <summary>
    /// Метод записи порядкового номера
    /// </summary>
    /// <param name="sequenceNumber">Порядковый номер</param>
    public void SetSequenceNumber(int sequenceNumber)
    {
        SequenceNumber = sequenceNumber;
    }

    /// <summary>
    /// Метод записи родителя
    /// </summary>
    /// <param name="parent">Родитель</param>
    public void SetParent(SectionInformationArticle parent)
    {
        ParentId = parent.Id;
        Parent = parent;
    }
}