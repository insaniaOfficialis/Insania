using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.Geography;

/// <summary>
/// Модель сущности географического объекта
/// </summary>
[Table("re_geographical_objects")]
[Comment("Географические объекты")]
public class GeographicalObject : Reestr
{
    /// <summary>
    /// Наименование
    /// </summary>
    [Column("name")]
    [Comment("Наименование")]
    public string Name { get; private set; }

    /// <summary>
    /// Цвет на карте
    /// </summary>
    [Column("color_on_map")]
    [Comment("Цвет на карте")]
    public string ColorOnMap { get; private set; }

    /// <summary>
    /// Ссылка на тип
    /// </summary>
    [Column("type_id")]
    [Comment("Ссылка на тип")]
    public long TypeId { get; private set; }

    /// <summary>
    /// Навигационное свойство типа
    /// </summary>
    public TypeGeographicalObject Type { get; private set; }

    /// <summary>
    /// Ссылка на родителя
    /// </summary>
    [Column("parent_id")]
    [Comment("Ссылка на родителя")]
    public long? ParentId { get; private set; }

    /// <summary>
    /// Навигационное свойство родителя
    /// </summary>
    public GeographicalObject? Parent { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности географического объекта
    /// </summary>
    public GeographicalObject() : base()
    {
        Name = string.Empty;
        ColorOnMap = string.Empty;
        Type = new();
    }

    /// <summary>
    /// Конструктор модели сущности географического объекта без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="colorOnMap">Цвет на карте</param>
    /// <param name="type">Ссылка на тип</param>
    /// <param name="parent">Ссылка на родителя</param>
    public GeographicalObject(string user, bool isSystem, string name, string colorOnMap, TypeGeographicalObject type,
        GeographicalObject? parent) : base(user, isSystem)
    {
        Name = name;
        ColorOnMap = colorOnMap;
        TypeId = type.Id;
        Type = type;
        ParentId = parent?.Id;
        Parent = parent;
    }

    /// <summary>
    /// Конструктор модели сущности географического объекта с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="colorOnMap">Цвет на карте</param>
    /// <param name="type">Ссылка на тип</param>
    /// <param name="parent">Ссылка на родителя</param>
    public GeographicalObject(long id, string user, bool isSystem, string name, string colorOnMap, 
        TypeGeographicalObject type, GeographicalObject? parent) : base(id, user, isSystem)
    {
        Name = name;
        ColorOnMap = colorOnMap;
        TypeId = type.Id;
        Type = type;
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
    /// Метод записи цвета на карте
    /// </summary>
    /// <param name="colorOnMap">Цвет на карте</param>
    public void SetColorOnMap(string colorOnMap)
    {
        ColorOnMap = colorOnMap;
    }

    /// <summary>
    /// Метод записи типа
    /// </summary>
    /// <param name="type">Ссылка на тип</param>
    public void SetType(TypeGeographicalObject type)
    {
        TypeId = type.Id;
        Type = type;
    }

    /// <summary>
    /// Метод записи родителя
    /// </summary>
    /// <param name="parent">Ссылка на родителя</param>
    public void SetParent(GeographicalObject parent)
    {
        ParentId = parent.Id;
        Parent = parent;
    }
}