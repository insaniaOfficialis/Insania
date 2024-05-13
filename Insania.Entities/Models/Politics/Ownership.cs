using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Entities.Models.Politics;

/// <summary>
/// Модель сущности владения
/// </summary>
[Table("re_ownerships")]
[Comment("Владения")]
public class Ownership : Reestr
{
    /// <summary>
    /// Наименование
    /// </summary>
    [Column("name")]
    [Comment("Наименование")]
    public string Name { get; private set; }

    /// <summary>
    /// Номер на карте
    /// </summary>
    [Column("number_on_map")]
    [Comment("Номер на карте")]
    public string NumberOnMap { get; private set; }

    /// <summary>
    /// Цвет на карте
    /// </summary>
    [Column("color_on_map")]
    [Comment("Цвет на карте")]
    public string ColorOnMap { get; private set; }

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
    /// Ссылка на родителя
    /// </summary>
    [Column("parent_id")]
    [Comment("Ссылка на родителя")]
    public long ParentId { get; private set; }

    /// <summary>
    /// Навигационное свойство родителя
    /// </summary>
    public Ownership Parent { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности владения
    /// </summary>
    public Ownership() : base()
    {
        Name = string.Empty;
        NumberOnMap = string.Empty;
        ColorOnMap = string.Empty;
        Organization = new();
        Parent = new();
    }

    /// <summary>
    /// Конструктор модели сущности владения без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="numberOnMap">Номер на карте/param>
    /// <param name="colorOnMap">Цвет на карте</param>
    /// <param name="organization">Ссылка на организацию</param>
    /// <param name="parent">Ссылка на родителя</param>
    public Ownership(string user, bool isSystem, string name, string numberOnMap, string colorOnMap, 
        Organization organization, Ownership parent) : base(user, isSystem)
    {
        Name = name;
        NumberOnMap = numberOnMap;
        ColorOnMap = colorOnMap;
        OrganizationId = organization.Id;
        Organization = organization;
        ParentId = parent.Id;
        Parent = parent;
    }

    /// <summary>
    /// Конструктор модели сущности владения с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="numberOnMap">Номер на карте/param>
    /// <param name="colorOnMap">Цвет на карте</param>
    /// <param name="organization">Ссылка на организацию</param>
    /// <param name="parent">Ссылка на родителя</param>
    public Ownership(long id, string user, bool isSystem, string name, string numberOnMap, string colorOnMap,
        Organization organization, Ownership parent) : base(id, user, isSystem)
    {
        Name = name;
        NumberOnMap = numberOnMap;
        ColorOnMap = colorOnMap;
        OrganizationId = organization.Id;
        Organization = organization;
        ParentId = parent.Id;
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
    /// Метод записи номера на карте
    /// </summary>
    /// <param name="numberOnMap">Номер на карте</param>
    public void SetNumberOnMap(string numberOnMap)
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
    /// Метод записи организации
    /// </summary>
    /// <param name="organization">Ссылка на организацию</param>
    public void SetOrganization(Organization organization)
    {
        OrganizationId = organization.Id;
        Organization = organization;
    }

    /// <summary>
    /// Метод записи родителя
    /// </summary>
    /// <param name="parent">Ссылка на родителя</param>
    public void SetParent(Ownership parent)
    {
        ParentId = parent.Id;
        Parent = parent;
    }
}