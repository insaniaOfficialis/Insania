using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Database.Entities.Users;
using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.Feedback;

/// <summary>
/// Модель сущности обратной связи
/// </summary>
[Table("re_feedback")]
[Comment("Обратная связь")]
public class Feedback : Reestr
{
    /// <summary>
    /// Ссылка на тип
    /// </summary>
    [Column("type_id")]
    [Comment("Ссылка на тип")]
    public long TypeId { get; private set; }

    /// <summary>
    /// Навигационное свойство типа
    /// </summary>
    public TypeFeedback Type { get; private set; }

    /// <summary>
    /// Ссылка на статус
    /// </summary>
    [Column("status_id")]
    [Comment("Ссылка на статус")]
    public long StatusId { get; private set; }

    /// <summary>
    /// Навигационное свойство статуса
    /// </summary>
    public StatusFeedback Status { get; private set; }

    /// <summary>
    /// Ссылка на автора
    /// </summary>
    [Column("author_id")]
    [Comment("Ссылка на автора")]
    public long? AuthorId { get; private set; }

    /// <summary>
    /// Навигационное свойство автора
    /// </summary>
    public User? Author { get; private set; }

    /// <summary>
    /// Текст
    /// </summary>
    [Column("text")]
    [Comment("Текст")]
    public string Text { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности обратной связи
    /// </summary>
    public Feedback() : base()
    {
        Text = string.Empty;
        Type = new();
        Status = new();
    }

    /// <summary>
    /// Конструктор сущности обратной связи без id
    /// </summary>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="type">Тип</param>
    /// <param name="status">Статус</param>
    /// <param name="author">Автор</param>
    /// <param name="text">Текст</param>
    public Feedback(string user, bool isSystem, TypeFeedback type, StatusFeedback status, User? author, string text) : base(user, isSystem)
    {
        TypeId = type.Id;
        Type = type;
        StatusId = status.Id;
        Status = status;
        AuthorId = author?.Id;
        Author = author;
        Text = text;
    }

    /// <summary>
    /// Конструктор сущности обратной связи без id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="type">Тип</param>
    /// <param name="status">Статус</param>
    /// <param name="author">Автор</param>
    /// <param name="text">Текст</param>
    public Feedback(long id, string user, bool isSystem, TypeFeedback type, StatusFeedback status, User? author, string text) : base(id, user, isSystem)
    {
        TypeId = type.Id;
        Type = type;
        StatusId = status.Id;
        Status = status;
        AuthorId = author?.Id;
        Author = author;
        Text = text;
    }

    /// <summary>
    /// Метод записи автора
    /// </summary>
    /// <param name="author">Автор</param>
    public void SetAuthor(User author)
    {
        AuthorId = author.Id;
        Author = author;
    }

    /// <summary>
    /// Метод записи текст
    /// </summary>
    /// <param name="text">Текст</param>
    public void SetText(string text)
    {
        Text = text;
    }

    /// <summary>
    /// Метод записи типа
    /// </summary>
    /// <param name="type">Тип</param>
    public void SetType(TypeFeedback type)
    {
        TypeId = type.Id;
        Type = type;
    }

    /// <summary>
    /// Метод записи статуса
    /// </summary>
    /// <param name="status">Статус</param>
    public void SetStatus(StatusFeedback status)
    {
        StatusId = status.Id;
        Status = status;
    }
}