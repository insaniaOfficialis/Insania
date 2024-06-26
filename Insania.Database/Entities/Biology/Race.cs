﻿using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.Biology;

/// <summary>
/// Модель сущности рас
/// </summary>
[Table("dir_races")]
[Comment("Расы")]
public class Race : Guide
{
    /// <summary>
    /// Простой конструктор модели сущности рас
    /// </summary>
    public Race() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности рас без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    public Race(string user, string name) : base(user, name)
    {

    }

    /// <summary>
    /// Конструктор модели сущности рас с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    public Race(long id, string user, string name) : base(id, user, name)
    {

    }
}