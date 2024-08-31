using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.Feedback;

/// <summary>
/// Модель сущности типа обратной связи
/// </summary>
[Table("dir_types_feedback")]
[Comment("Типы обратной связи")]
public class TypeFeedback : Guide
{
    /// <summary>
    /// Ссылка на родителя
    /// </summary>
    [Column("parent_id")]
    [Comment("Ссылка на родителя")]
    public long? ParentId { get; private set; }

    /// <summary>
    /// Навигационное свойство родителя
    /// </summary>
    public TypeFeedback? Parent { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности типа обратной связи
    /// </summary>
    public TypeFeedback() : base()
    {
        
    }

    /// <summary>
    /// Конструктор модели сущности типа обратной связи без id
    /// </summary>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    /// <param name="parent">Родитель</param>
    public TypeFeedback(string user, string name, TypeFeedback? parent) : base(user, name)
    {
        ParentId = parent?.Id;
        Parent = parent;
    }

    /// <summary>
    /// Конструктор модели сущности типа обратной связи c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    /// <param name="parent">Родитель</param>
    public TypeFeedback(long id, string user, string name, TypeFeedback? parent) : base(id, user, name)
    {
        ParentId = parent?.Id;
        Parent = parent;
    }

    /// <summary>
    /// Метод записи родителя
    /// </summary>
    /// <param name="parent">Родитель</param>
    public void SetParent(TypeFeedback parent)
    {
        ParentId = parent.Id;
        Parent = parent;
    }
}