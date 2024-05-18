using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;
using Insania.Database.Entities.Politics;

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
    /// Признак верховности
    /// </summary>
    [Column("is_paramount")]
    [Comment("Признак верховности")]
    public bool IsParamount { get; private set; }

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
    /// <param name="isParamount">Признак верховности</param>
    public Chapter(string user, bool isSystem, string name, Country? country,
        bool isParamount) : base(user, isSystem)
    {
        Name = name;
        CountryId = country?.Id;
        Country = country;
        IsParamount = isParamount;
    }

    /// <summary>
    /// Конструктор модели сущности капитула с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="country">Ссылка на страну</param>
    /// <param name="isParamount">Признак верховности</param>
    public Chapter(long id, string user, bool isSystem, string name, Country? country,
        bool isParamount) : base(id, user, isSystem)
    {
        Name = name;
        CountryId = country?.Id;
        Country = country;
        IsParamount = isParamount;
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
    /// Метод записи признака верховности
    /// </summary>
    /// <param name="isParamount">Признак верховности</param>
    public void SetIsParamount(bool isParamount)
    {
        IsParamount = isParamount;
    }
}