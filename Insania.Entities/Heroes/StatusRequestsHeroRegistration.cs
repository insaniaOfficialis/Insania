using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Entities.Heroes;

/// <summary>
/// Модель сущности статуса заявок на регистрацию персонажа
/// </summary>
[Table("dir_statuses_requests_hero_registration")]
[Comment("Статусы заявок на регистрацию персонажа")]
public class StatusRequestsHeroRegistration : Guide
{
    /// <summary>
    /// Ссылка на предыдущий статус
    /// </summary>
    [Column("previous_id")]
    [Comment("Ссылка на предыдущий статус")]
    public long PreviousId { get; private set; }

    /// <summary>
    /// Навигационное свойство предыдущего статуса
    /// </summary>
    public StatusRequestsHeroRegistration Previous { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности статуса заявок на регистрацию персонажа
    /// </summary>
    public StatusRequestsHeroRegistration() : base()
    {
        Previous = new();
    }

    /// <summary>
    /// Конструктор модели сущности статуса заявок на регистрацию персонажа без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    /// <param name="previous">Ссылка на предыдущий статус</param>
    public StatusRequestsHeroRegistration(string user, string name, 
        StatusRequestsHeroRegistration previous) : base(user, name)
    {
        PreviousId = previous.Id;
        Previous = previous;
    }

    /// <summary>
    /// Конструктор модели сущности статуса заявок на регистрацию персонажа c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    /// <param name="previous">Ссылка на предыдущий статус</param>
    public StatusRequestsHeroRegistration(long id, string user, string name,
        StatusRequestsHeroRegistration previous) : base(id, user, name)
    {
        PreviousId = previous.Id;
        Previous = previous;
    }

    /// <summary>
    /// Метод записи предыдущего статуса
    /// </summary>
    /// <param name="previous">Ссылка на предыдущий статус</param>
    public void SetPrevious(StatusRequestsHeroRegistration previous)
    { 
        PreviousId = previous.Id;
        Previous = previous;
    }
}