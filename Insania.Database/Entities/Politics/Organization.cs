using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Database.Entities.Politics;

/// <summary>
/// Модель сущности организации
/// </summary>
[Table("re_organizations")]
[Comment("Организации")]
public class Organization : Reestr
{
    /// <summary>
    /// Наименование
    /// </summary>
    [Column("name")]
    [Comment("Наименование")]
    public string Name { get; private set; }

    /// <summary>
    /// Ссылка на тип
    /// </summary>
    [Column("type_id")]
    [Comment("Ссылка на тип")]
    public long TypeId { get; private set; }

    /// <summary>
    /// Навигационное свойство типа
    /// </summary>
    public TypeOrganization Type { get; private set; }

    /// <summary>
    /// Ссылка на родителя
    /// </summary>
    [Column("parent_id")]
    [Comment("Ссылка на родителя")]
    public long ParentId { get; private set; }

    /// <summary>
    /// Навигационное свойство родителя
    /// </summary>
    public Organization Parent { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности организации
    /// </summary>
    public Organization() : base()
    {
        Name = string.Empty;
        Type = new();
        Parent = new();
    }

    /// <summary>
    /// Конструктор модели сущности организации без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="type">Ссылка на тип</param>
    /// <param name="parent">Ссылка на родителя</param>
    public Organization(string user, bool isSystem, string name, TypeOrganization type, Organization parent) 
        : base(user, isSystem)
    {
        Name = name;
        TypeId = type.Id;
        Type = type;
        ParentId = parent.Id;
        Parent = parent;
    }

    /// <summary>
    /// Конструктор модели сущности организации с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="type">Ссылка на тип</param>
    /// <param name="parent">Ссылка на родителя</param>
    public Organization(long id, string user, bool isSystem, string name, TypeOrganization type, Organization parent) 
        : base(id, user, isSystem)
    {
        Name = name;
        TypeId = type.Id;
        Type = type;
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
    /// Метод записи типа
    /// </summary>
    /// <param name="type">Ссылка на тип</param>
    public void SetType(TypeOrganization type)
    {
        TypeId = type.Id;
        Type = type;
    }

    /// <summary>
    /// Метод записи родителя
    /// </summary>
    /// <param name="type">Ссылка на родителя</param>
    public void SetParent(Organization parent)
    {
        ParentId = parent.Id;
        Parent = parent;
    }
}