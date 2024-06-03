using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.Sociology;

/// <summary>
/// Модель сущности префикса имени
/// </summary>
[Table("dir_prefixes_names")]
[Comment("Префиксы имён")]
public class PrefixName : Guide
{
    /// <summary>
    /// Навигационное свойство префиксов имён наций
    /// </summary>
    public ICollection<PrefixNameNation>? PrefixNameNations { get; set; }

    /// <summary>
    /// Простой конструктор модели сущности префикса имени
    /// </summary>
    public PrefixName() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности префикса имени без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    public PrefixName(string user, string name) : base(user, name)
    {

    }

    /// <summary>
    /// Конструктор модели сущности префикса имени с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    public PrefixName(long id, string user, string name) : base(id, user, name)
    {

    }
}