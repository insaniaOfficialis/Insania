using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Database.Entities.Politics;

/// <summary>
/// Модель сущности региона
/// </summary>
[Table("re_regions")]
[Comment("Регионы")]
public class Region : Reestr
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
    /// Простой конструктор модели сущности региона
    /// </summary>
    public Region() : base()
    {
        Name = string.Empty;
        NumberOnMap = string.Empty;
        ColorOnMap = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности региона без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="numberOnMap">Номер на карте/param>
    /// <param name="colorOnMap">Цвет на карте</param>
    public Region(string user, bool isSystem, string name, string numberOnMap, string colorOnMap) : base(user,
        isSystem)
    {
        Name = name;
        NumberOnMap = numberOnMap;
        ColorOnMap = colorOnMap;
    }

    /// <summary>
    /// Конструктор модели сущности региона с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="numberOnMap">Номер на карте/param>
    /// <param name="colorOnMap">Цвет на карте</param>
    public Region(long id, string user, bool isSystem, string name, string numberOnMap, string colorOnMap) : base(id,
        user, isSystem)
    {
        Name = name;
        NumberOnMap = numberOnMap;
        ColorOnMap = colorOnMap;
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
}