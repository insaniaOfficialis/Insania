using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;


namespace Insania.Database.Entities.Politics;

/// <summary>
/// Модель сущности фракции
/// </summary>
[Table("re_fractions")]
[Comment("Фракции")]
public class Fraction : Reestr
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
    /// Функции
    /// </summary>
    [Column("functions")]
    [Comment("Функции")]
    public string Functions { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности фракции
    /// </summary>
    public Fraction() : base()
    {
        Name = string.Empty;
        ColorOnMap = string.Empty;
        Functions = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности фракции без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="colorOnMap">Цвет на карте</param>
    /// <param name="functions">Функции</param>
    public Fraction(string user, bool isSystem, string name, string colorOnMap, string functions) : base(user, isSystem)
    {
        Name = name;
        ColorOnMap = colorOnMap;
        Functions = functions;
    }

    /// <summary>
    /// Конструктор модели сущности фракции с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="colorOnMap">Цвет на карте</param>
    /// <param name="functions">Функции</param>
    public Fraction(long id, string user, bool isSystem, string name, string colorOnMap, string functions) : base(id,
        user, isSystem)
    {
        Name = name;
        ColorOnMap = colorOnMap;
        Functions = functions;
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
    /// Метод записи функции
    /// </summary>
    /// <param name="functions">Функции</param>
    public void SetFunction(string functions)
    {
        Functions = functions;
    }
}