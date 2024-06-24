using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.InformationArticles;

/// <summary>
/// Модель сущности детальной части информационной статьи
/// </summary>
[Table("re_details_information_articles")]
[Comment("Детальные части информационных статей")]
public class DetailInformationArticle : Reestr
{
    /// <summary>
    /// Оглавление
    /// </summary>
    [Column("title")]
    [Comment("Оглавление")]
    public string Title { get; private set; }

    /// <summary>
    /// Текст
    /// </summary>
    [Column("text")]
    [Comment("Текст")]
    public string Text { get; private set; }

    /// <summary>
    /// Порядковый номер
    /// </summary>
    [Column("sequence_number")]
    [Comment("Порядковый номер")]
    public int? SequenceNumber { get; private set; }

    /// <summary>
    /// Ссылка на оглавление
    /// </summary>
    [Column("header_id")]
    [Comment("Ссылка на оглавление")]
    public long HeaderId { get; private set; }

    /// <summary>
    /// Навигационное свойство оглавления
    /// </summary>
    public HeaderInformationArticle Header { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности детальной части информационной статьи
    /// </summary>
    public DetailInformationArticle() : base()
    {
        Title = string.Empty;
        Text = string.Empty;
        Header = new();
    }

    /// <summary>
    /// Конструктор модели сущности детальной части информационной статьи без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="title">Оглавление</param>
    /// <param name="text">Текст</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="sequenceNumber">Порядковый номер</param>
    /// <param name="header">Оглавление</param>
    public DetailInformationArticle(string user, bool isSystem, string title, string text, int sequenceNumber,
        HeaderInformationArticle header) : base(user, isSystem)
    {
        Title = title;
        Text = text;
        SequenceNumber = sequenceNumber;
        Header = header;
        HeaderId = header.Id;
    }

    /// <summary>
    /// Конструктор модели сущности детальной части информационной статьи с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="title">Оглавление</param>
    /// <param name="text">Текст</param>
    /// <param name="sequenceNumber">Порядковый номер</param>
    /// <param name="header">Оглавление</param>
    public DetailInformationArticle(long id, string user, bool isSystem, string title, string text, int sequenceNumber,
        HeaderInformationArticle header) : base(id, user, isSystem)
    {
        Title = title;
        Text = text;
        SequenceNumber = sequenceNumber;
        Header = header;
        HeaderId = header.Id;
    }

    /// <summary>
    /// Метод записи оглавления
    /// </summary>
    /// <param name="title">Оглавление</param>
    public void SetTitle(string title)
    {
        Title = title;
    }

    /// <summary>
    /// Метод записи текста
    /// </summary>
    /// <param name="text">Текст</param>
    public void SetText(string text)
    {
        Text = text;
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
    /// Метод записи оглавления
    /// </summary>
    /// <param name="header">Оглавление</param>
    public void SetHeader(HeaderInformationArticle header)
    {
        Header = header;
        HeaderId = header.Id;
    }
}