using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using BaseEntity = Insania.Entities.OutCategories.Base;

namespace Insania.Database.Entities.Politics;

/// <summary>
/// Модель сущности региона владения
/// </summary>
[Table("un_regions_ownerships")]
[Comment("Регионы владений")]
public class RegionOwnership : BaseEntity
{
    /// <summary>
    /// Ссылка на регион
    /// </summary>
    [Column("region_id")]
    [Comment("Ссылка на регион")]
    public long RegionId { get; private set; }

    /// <summary>
    /// Навигационное свойство региона
    /// </summary>
    public Region Region { get; private set; }

    /// <summary>
    /// Ссылка на владение
    /// </summary>
    [Column("ownership_id")]
    [Comment("Ссылка на владение")]
    public long OwnershipId { get; private set; }

    /// <summary>
    /// Навигационное свойство владения
    /// </summary>
    public Ownership Ownership { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности региона владения
    /// </summary>
    public RegionOwnership() : base()
    {
        Region = new();
        Ownership = new();
    }

    /// <summary>
    /// Конструктор модели сущности региона владения без id и порядкового номера
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="region">Регион</param>
    /// <param name="ownership">Владение</param>
    public RegionOwnership(string user, Region region, Ownership ownership) : base(user)
    {
        RegionId = region.Id;
        Region = region;
        OwnershipId = ownership.Id;
        Ownership = ownership;
    }

    /// <summary>
    /// Конструктор модели сущности региона владения с id и без порядкового номера
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="region">Регион</param>
    /// <param name="ownership">Владение</param>
    public RegionOwnership(long id, string user, Region region, Ownership ownership) : base(id, user)
    {
        RegionId = region.Id;
        Region = region;
        OwnershipId = ownership.Id;
        Ownership = ownership;
    }
}