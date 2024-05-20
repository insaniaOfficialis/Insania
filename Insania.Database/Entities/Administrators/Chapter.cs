using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Database.Entities.Politics;
using Insania.Entities.Base;

namespace Insania.Database.Entities.Administrators;

/// <summary>
/// Модель сущности капитула
/// </summary>
[Table("re_chapters")]
[Comment("Капитулы")]
public class Chapter : Reestr
{
    /// <summary>
    /// Наименование
    /// </summary>
    [Column("name")]
    [Comment("Наименование")]
    public string Name { get; private set; }

    /// <summary>
    /// Ссылка на страну
    /// </summary>
    [Column("country_id")]
    [Comment("Ссылка на страну")]
    public long? CountryId { get; private set; }

    /// <summary>
    /// Навигационное свойство страны
    /// </summary>
    public Country? Country { get; private set; }

    /// <summary>
    /// Ссылка на родителя
    /// </summary>
    [Column("parent_id")]
    [Comment("Ссылка на родителя")]
    public long? ParentId { get; private set; }

    /// <summary>
    /// Навигационное свойство родителя
    /// </summary>
    public Chapter? Parent { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности капитула
    /// </summary>
    public Chapter() : base()
    {
        Name = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности капитула без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="country">Ссылка на страну</param>
    /// <param name="parent">Ссылка на родителя</param>
    public Chapter(string user, bool isSystem, string name, Country? country, Chapter? parent) : base(user, isSystem)
    {
        Name = name;
        CountryId = country?.Id;
        Country = country;
        ParentId = parent?.Id;
        Parent = parent;
    }

    /// <summary>
    /// Конструктор модели сущности капитула с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="country">Ссылка на страну</param>
    /// <param name="parent">Ссылка на родителя</param>
    public Chapter(long id, string user, bool isSystem, string name, Country? country, Chapter? parent) : base(id, user, isSystem)
    {
        Name = name;
        CountryId = country?.Id;
        Country = country;
        ParentId = parent?.Id;
        Parent = parent;
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
    /// Метод записи страны
    /// </summary>
    /// <param name="country">Ссылка на страну</param>
    public void SetCountry(Country country)
    {
        CountryId = country.Id;
        Country = country;
    }

    /// <summary>
    /// Метод записи родителя
    /// </summary>
    /// <param name="type">Ссылка на родителя</param>
    public void SetParent(Chapter parent)
    {
        ParentId = parent.Id;
        Parent = parent;
    }
}