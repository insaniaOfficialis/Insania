using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.Geography;

/// <summary>
/// Модель сущности типа географического объекта
/// </summary>
[Table("dir_types_geographical_objects")]
[Comment("Типы географического объекта")]
public class TypeGeographicalObject : Guide
{
    /// <summary>
    /// Простой конструктор модели сущности типа географического объекта
    /// </summary>
    public TypeGeographicalObject() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности типа географического объекта без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    public TypeGeographicalObject(string user, string name) : base(user, name)
    {

    }

    /// <summary>
    /// Конструктор модели сущности типа географического объекта с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    public TypeGeographicalObject(long id, string user, string name) : base(id, user, name)
    {

    }
}