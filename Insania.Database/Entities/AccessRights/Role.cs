using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Insania.Database.Entities.AccessRights;

/// <summary>
/// Модели сущности роли
/// </summary>
[Table("sys_roles")]
[Comment("Роли")]
public class Role : IdentityRole<long>
{
    /// <summary>
    /// Простой конструктор модели сущности роли
    /// </summary>
    public Role()
    {

    }

    /// <summary>
    /// Конструктор модели сущности роли без id
    /// </summary>
    /// <param name="name">Наименование</param>
    public Role(string name) : this()
    {
        Name = name;
    }

    /// <summary>
    /// Конструктор модели сущности роли с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="name">Наименование</param>
    public Role(long id, string name) : this(name)
    {
        Id = id;
    }

    /// <summary>
    /// Метод записи наименования
    /// </summary>
    /// <param name="name">Наименование</param>
    public void SetName(string name)
    {
        Name = name;
    }
}