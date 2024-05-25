using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Insania.Entities.OutCategories;

/// <summary>
/// Модель сущности реестра
/// </summary>
public abstract class Reestr : Base
{
    /// <summary>
    ///	Признак системной записи
    /// </summary>
    [Column("is_system")]
    [Comment("Признак системной записи")]
    public bool IsSystem { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности реестра
    /// </summary>
    public Reestr(): base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности реестра без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    public Reestr(string user, bool isSystem): base(user)
    {
        IsSystem = isSystem;
    }

    /// <summary>
    /// Конструктор модели сущности реестра c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="isSystem">Признак системной записи</param>
    public Reestr(long id, string user, bool isSystem): 
        base(id, user)
    {
        IsSystem = isSystem;
    }

    /// <summary>
    /// Метод записи признака системности записи
    /// </summary>
    /// <param name="isSystem">Признак системной записи</param>
    public void SetIsSystem(bool isSystem)
    { 
        IsSystem = isSystem; 
    }
}