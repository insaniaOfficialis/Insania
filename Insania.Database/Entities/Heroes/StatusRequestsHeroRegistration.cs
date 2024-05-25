using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.Heroes;

/// <summary>
/// Модель сущности статуса заявки на регистрацию персонажа
/// </summary>
[Table("dir_statuses_requests_heroes_registration")]
[Comment("Статусы заявок на регистрацию персонажей")]
public class StatusRequestHeroRegistration : Guide
{
    /// <summary>
    /// Ссылка на предыдущий статус
    /// </summary>
    [Column("previous_id")]
    [Comment("Ссылка на предыдущий статус")]
    public long? PreviousId { get; private set; }

    /// <summary>
    /// Навигационное свойство предыдущего статуса
    /// </summary>
    public StatusRequestHeroRegistration? Previous { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности статуса заявки на регистрацию персонажа
    /// </summary>
    public StatusRequestHeroRegistration() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности статуса заявки на регистрацию персонажа без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    /// <param name="previous">Ссылка на предыдущий статус</param>
    public StatusRequestHeroRegistration(string user, string name, StatusRequestHeroRegistration? previous) : base(user, name)
    {
        PreviousId = previous?.Id;
        Previous = previous;
    }

    /// <summary>
    /// Конструктор модели сущности статуса заявки на регистрацию персонажа c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    /// <param name="previous">Ссылка на предыдущий статус</param>
    public StatusRequestHeroRegistration(long id, string user, string name, StatusRequestHeroRegistration? previous) : base(id,
        user, name)
    {
        PreviousId = previous?.Id;
        Previous = previous;
    }

    /// <summary>
    /// Метод записи предыдущего статуса
    /// </summary>
    /// <param name="previous">Ссылка на предыдущий статус</param>
    public void SetPrevious(StatusRequestHeroRegistration previous)
    {
        PreviousId = previous.Id;
        Previous = previous;
    }
}