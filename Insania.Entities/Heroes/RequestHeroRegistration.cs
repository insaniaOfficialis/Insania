using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Entities.Heroes;

/// <summary>
/// Модель сущности заявки на регистрацию персонажа
/// </summary>
[Table("re_requests_heroes_registration")]
[Comment("Заявки на регистрацию персонажа")]
public class RequestHeroRegistration : Reestr
{
    /// <summary>
    /// Ссылка на персонажа
    /// </summary>
    [Column("hero_id")]
    [Comment("Ссылка на персонажа")]
    public long HeroId { get; private set; }

    /// <summary>
    /// Навигационное свойство персонажа
    /// </summary>
    public Hero Hero { get; private set; }

    /// <summary>
    /// Ссылка на статуса
    /// </summary>
    [Column("status_id")]
    [Comment("Ссылка на статус")]
    public long StatusId { get; private set; }

    /// <summary>
    /// Навигационное свойство статуса
    /// </summary>
    public StatusRequestsHeroRegistration Status { get; private set; }


}