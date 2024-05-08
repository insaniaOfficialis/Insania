using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Entities.Appearance;

/// <summary>
/// Модель сущности типов лиц
/// </summary>
[Table("dir_types_face")]
[Comment("Типы лиц")]
public class TypeFace : Guide
{
    /// <summary>
    /// Простой конструктор модели сущности типов лиц
    /// </summary>
    public TypeFace() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности типов лиц без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    public TypeFace(string user, string name) : base(user, name)
    {

    }

    /// <summary>
    /// Конструктор модели сущности типов лиц с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    public TypeFace(long id, string user, string name) : base(id, user, name)
    {

    }
}