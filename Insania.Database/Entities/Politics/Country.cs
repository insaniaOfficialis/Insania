using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Database.Entities.Politics;

/// <summary>
/// Модель сущности страны
/// </summary>
[Table("re_countries")]
[Comment("Страны")]
public class Country : Reestr
{
    /// <summary>
    /// Номер на карте
    /// </summary>
    [Column("number_on_map")]
    [Comment("Номер на карте")]
    public int NumberOnMap { get; private set; }

    /// <summary>
    /// Цвет на карте
    /// </summary>
    [Column("color_on_map")]
    [Comment("Цвет на карте")]
    public string ColorOnMap { get; private set; }

    /// <summary>
    /// Язык для названий
    /// </summary>
    [Column("language_for_personal_names")]
    [Comment("Язык для названий")]
    public string LanguageForNames { get; private set; }

    /// <summary>
    /// Ссылка на организацию
    /// </summary>
    [Column("organization_id")]
    [Comment("Ссылка на организацию")]
    public long OrganizationId { get; private set; }

    /// <summary>
    /// Навигационное свойство организации
    /// </summary>
    public Organization Organization { get; private set; }

    /// <summary>
    /// Код
    /// </summary>
    [Column("code")]
    [Comment("Код")]
    public string Code { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности страны
    /// </summary>
    public Country() : base()
    {
        ColorOnMap = string.Empty;
        LanguageForNames = string.Empty;
        Organization = new();
        Code = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности страны без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="numberOnMap">Номер на карте/param>
    /// <param name="colorOnMap">Цвет на карте</param>
    /// <param name="languageForNames">Язык для названий</param>
    /// <param name="organization">Организация</param>
    /// <param name="code">Код</param>
    public Country(string user, bool isSystem, int numberOnMap, string colorOnMap, string languageForNames, 
        Organization organization, string code) : base(user, isSystem)
    {
        NumberOnMap = numberOnMap;
        ColorOnMap = colorOnMap;
        LanguageForNames = languageForNames;
        OrganizationId = organization.Id;
        Organization = organization;
        Code = code;
    }

    /// <summary>
    /// Конструктор модели сущности страны с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="numberOnMap">Номер на карте/param>
    /// <param name="colorOnMap">Цвет на карте</param>
    /// <param name="languageForNames">Язык для названий</param>
    /// <param name="organization">Ссылка на организацию</param>
    /// <param name="code">Код</param>
    public Country(long id, string user, bool isSystem, int numberOnMap, string colorOnMap, string languageForNames, 
        Organization organization, string code) : base(id, user, isSystem)
    {
        NumberOnMap = numberOnMap;
        ColorOnMap = colorOnMap;
        LanguageForNames = languageForNames;
        OrganizationId = organization.Id;
        Organization = organization;
        Code = code;
    }

    /// <summary>
    /// Метод записи номера на карте
    /// </summary>
    /// <param name="numberOnMap">Номер на карте</param>
    public void SetNumberOnMap(int numberOnMap)
    {
        NumberOnMap = numberOnMap;
    }

    /// <summary>
    /// Метод записи цвета на карте
    /// </summary>
    /// <param name="colorOnMap">Цвет на карте</param>
    public void SetColorOnMap(string colorOnMap)
    {
        ColorOnMap = colorOnMap;
    }

    /// <summary>
    /// Метод записи языка для названий
    /// </summary>
    /// <param name="languageForNames">Язык для названий</param>
    public void SetLanguageForNames(string languageForNames)
    {
        LanguageForNames = languageForNames;
    }

    /// <summary>
    /// Метод записи организации
    /// </summary>
    /// <param name="organization">Организация</param>
    public void SetOrganization(Organization organization)
    {
        OrganizationId = organization.Id;
        Organization = organization;
    }

    /// <summary>
    /// Метод записи кода
    /// </summary>
    /// <param name="code">Код</param>
    public void SetCode(string code)
    {
        Code = code;
    }
}