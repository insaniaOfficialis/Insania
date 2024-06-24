using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.InformationArticles;

/// <summary>
/// Модель сущности оглавления информационной статьи
/// </summary>
[Table("re_headers_information_articles")]
[Comment("Оглавления информационных статей")]
public class HeaderInformationArticle : Reestr
{
    /// <summary>
    /// Наименование
    /// </summary>
    [Column("name")]
    [Comment("Наименование")]
    public string Name { get; private set; }

    /// <summary>
    /// Порядковый номер
    /// </summary>
    [Column("sequence_number")]
    [Comment("Порядковый номер")]
    public int? SequenceNumber { get; private set; }

    /// <summary>
    /// Ссылка на раздел
    /// </summary>
    [Column("section_id")]
    [Comment("Ссылка на раздел")]
    public long SectionId { get; private set; }

    /// <summary>
    /// Навигационное свойство раздела
    /// </summary>
    public SectionInformationArticle Section { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности оглавления информационной статьи
    /// </summary>
    public HeaderInformationArticle() : base()
    {
        Name = string.Empty;
        Section = new();
    }

    /// <summary>
    /// Конструктор модели сущности оглавления информационной статьи без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="sequenceNumber">Порядковый номер</param>
    /// <param name="section">Раздел</param>
    public HeaderInformationArticle(string user, bool isSystem, string name, int sequenceNumber, SectionInformationArticle section) 
        : base(user, isSystem)
    {
        Name = name;
        SequenceNumber = sequenceNumber;
        Section = section;
        SectionId = section.Id;
    }

    /// <summary>
    /// Конструктор модели сущности оглавления информационной статьи с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="sequenceNumber">Порядковый номер</param>
    /// <param name="section">Раздел</param>
    public HeaderInformationArticle(long id, string user, bool isSystem, string name, int sequenceNumber, 
        SectionInformationArticle section) : base(id, user, isSystem)
    {
        Name = name;
        SequenceNumber = sequenceNumber;
        Section = section;
        SectionId = section.Id;
    }

    /// <summary>
    /// Метод записи наименования
    /// </summary>
    /// <param name="name">Наименование</param>
    public void SetName(string name)
    {
        Name = name;
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
    /// Метод записи раздела
    /// </summary>
    /// <param name="section">Раздел</param>
    public void SetSection(SectionInformationArticle section)
    {
        Section = section;
        SectionId = section.Id;
    }
}