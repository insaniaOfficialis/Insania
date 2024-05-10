using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Entities.Appearance;

/// <summary>
/// Модель сущности типа телосложения
/// </summary>
[Table("dir_types_body")]
[Comment("Типы телосложений")]
public class TypeBody : Guide
{
    /// <summary>
    /// Простой конструктор модели сущности типа телосложения
    /// </summary>
    public TypeBody() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности типа телосложения без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    public TypeBody(string user, string name) : base(user, name)
    {

    }

    /// <summary>
    /// Конструктор модели сущности типа телосложения с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    public TypeBody(long id, string user, string name) : base(id, user, name)
    {

    }
}