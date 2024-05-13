using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Entities.Models.Politics;

/// <summary>
/// Модель сущности типа организации
/// </summary>
[Table("dir_types_organizations")]
[Comment("Типы организаций")]
public class TypeOrganization : Guide
{
    /// <summary>
    /// Простой конструктор модели сущности типа организации
    /// </summary>
    public TypeOrganization() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности типа организации без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    public TypeOrganization(string user, string name) : base(user, name)
    {

    }

    /// <summary>
    /// Конструктор модели сущности типа организации с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    public TypeOrganization(long id, string user, string name) : base(id, user, name)
    {

    }
}