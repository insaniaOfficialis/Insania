using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;


namespace Insania.Database.Entities.Politics;

/// <summary>
/// Модель сущности фракции
/// </summary>
[Table("dir_fractions")]
[Comment("Фракции")]
public class Fraction : Guide
{
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
        ColorOnMap = string.Empty;
        Functions = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности фракции без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    /// <param name="colorOnMap">Цвет на карте</param>
    /// <param name="functions">Функции</param>
    public Fraction(string user, string name, string colorOnMap, string functions) : base(user, name)
    {
        ColorOnMap = colorOnMap;
        Functions = functions;
    }

    /// <summary>
    /// Конструктор модели сущности фракции с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    /// <param name="colorOnMap">Цвет на карте</param>
    /// <param name="functions">Функции</param>
    public Fraction(long id, string user, string name, string colorOnMap, string functions) : base(id,
        user, name)
    {
        ColorOnMap = colorOnMap;
        Functions = functions;
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