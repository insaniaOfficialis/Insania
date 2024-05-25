using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.System;

/// <summary>
/// Модель сущности параметра
/// </summary>
[Table("dir_parameters")]
[Comment("Параметры")]
public class Parameter : Guide
{
    /// <summary>
    /// Значение
    /// </summary>
    [Column("value")]
    [Comment("Значение")]
    public string? Value { get; set; }

    /// <summary>
    /// Простой конструктор модели сущности параметра
    /// </summary>
    public Parameter() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности параметра без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    /// <param name="value">Значение</param>
    public Parameter(string user, string name, string? value) : base(user, name)
    {
        Value = value;
    }

    /// <summary>
    /// Конструктор модели сущности параметра с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    /// <param name="value">Значение</param>
    public Parameter(long id, string user, string name, string? value) : base(id, user, name)
    {
        Value = value;

    }

    /// <summary>
    /// Метод записи значения
    /// </summary>
    /// <param name="value">Значение</param>
    public void SetValue(string? value)
    {
        Value = value;
    }
}