using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Entities.Appearance;

/// <summary>
/// Модель сущности цвета глаз
/// </summary>
[Table("dir_eye_colors")]
[Comment("Цвета глаз")]
public class EyeColor : Guide
{
    /// <summary>
    /// Rgb-модель цвета
    /// </summary>
    [Column("rgb")]
    [Comment("Rgb-модель цвета")]
    public string Rgb { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности цвета глаз
    /// </summary>
    public EyeColor() : base()
    {
        Rgb = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности рас цвета глаз id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    /// <param name="rgb">Rgb-модель цвета</param>
    public EyeColor(string user, string name, string rgb) : base(user, name)
    {
        Rgb = rgb;
    }

    /// <summary>
    /// Конструктор модели сущности цвета глаз с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="rgb">Rgb-модель цвета</param>
    public EyeColor(long id, string user, string name, string rgb) : base(id, user, name)
    {
        Rgb = rgb;
    }

    /// <summary>
    /// Метод записи rgb-модели цвета
    /// </summary>
    /// <param name="rgb">Rgb-модель цвета</param>
    public void SetRgb (string rgb)
    {
        Rgb = rgb;
    }
}