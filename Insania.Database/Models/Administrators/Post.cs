using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Entities.Models.Administrators;

/// <summary>
/// Модель сущности должности
/// </summary>
[Table("dir_posts")]
[Comment("Должности")]
public class Post : Guide
{
    /// <summary>
    /// Сфера деятельности
    /// </summary>
    [Column("scope_activity")]
    [Comment("Сфера деятельности")]
    public string ScopeActivity { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности должности
    /// </summary>
    public Post() : base()
    {
        ScopeActivity = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности должности без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    /// <param name="scopeActivity">Сфера деятельности</param>
    public Post(string user, string name, string scopeActivity) : base(user, name)
    {
        ScopeActivity = scopeActivity;
    }

    /// <summary>
    /// Конструктор модели сущности должности c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    /// <param name="scopeActivity">Сфера деятельности</param>
    public Post(long id, string user, string name, string scopeActivity) : base(id, user, name)
    {
        ScopeActivity = scopeActivity;
    }

    /// <summary>
    /// Метод записи сферы деятельности
    /// </summary>
    /// <param name="scopeActivity">Сфера деятельности</param>
    public void SetScopeActivity(string scopeActivity)
    {
        ScopeActivity = scopeActivity;
    }
}