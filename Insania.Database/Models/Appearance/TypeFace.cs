using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Entities.Models.Appearance;

/// <summary>
/// Модель сущности типа лица
/// </summary>
[Table("dir_types_faces")]
[Comment("Типы лиц")]
public class TypeFace : Guide
{
    /// <summary>
    /// Простой конструктор модели сущности типа лица
    /// </summary>
    public TypeFace() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности типа лица без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    public TypeFace(string user, string name) : base(user, name)
    {

    }

    /// <summary>
    /// Конструктор модели сущности типа лица с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    public TypeFace(long id, string user, string name) : base(id, user, name)
    {

    }
}