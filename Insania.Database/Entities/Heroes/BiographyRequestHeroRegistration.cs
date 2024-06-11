using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.Heroes;

/// <summary>
/// Модель сущности биографии заявки на регистрацию персонажа
/// </summary>
[Table("re_biographies_requests_heroes_registration")]
[Comment("Биографии заявок на регистрацию персонажей")]
public class BiographyRequestHeroRegistration : Reestr
{
    /// <summary>
    /// Ссылка на заявку
    /// </summary>
    [Column("request_id")]
    [Comment("Ссылка на заявку")]
    public long RequestId { get; private set; }

    /// <summary>
    /// Навигационное свойство заявки
    /// </summary>
    public RequestHeroRegistration Request { get; private set; }

    /// <summary>
    /// Ссылка на биографию персонажа
    /// </summary>
    [Column("biography_id")]
    [Comment("Ссылка на биографию персонажа")]
    public long BiographyId { get; private set; }

    /// <summary>
    /// Навигационное свойство биографии персонажа
    /// </summary>
    public BiographyHero Biography { get; private set; }

    /// <summary>
    /// Решение
    /// </summary>
    [Column("decision")]
    [Comment("Решение")]
    public bool? Decision { get; private set; }

    /// <summary>
    /// Комментарий
    /// </summary>
    [Column("comment")]
    [Comment("Комментарий")]
    public string? Comment { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности биографии заявки на регистрацию персонажа
    /// </summary>
    public BiographyRequestHeroRegistration() : base()
    {
        Request = new();
        Biography = new();
    }

    /// <summary>
    /// Конструктор модели сущности биографии заявки на регистрацию персонажа без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="request">Ссылка на заявку</param>
    /// <param name="biography">Ссылка на биографию</param>
    public BiographyRequestHeroRegistration(string user, bool isSystem, RequestHeroRegistration request,
        BiographyHero biography) : base(user, isSystem)
    {
        RequestId = request.Id;
        Request = request;
        BiographyId = biography.Id;
        Biography = biography;
    }

    /// <summary>
    /// Конструктор модели сущности биографии заявки на регистрацию персонажа c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="request">Ссылка на заявку</param>
    /// <param name="biography">Ссылка на биографию</param>
    public BiographyRequestHeroRegistration(long id, string user, bool isSystem, RequestHeroRegistration request,
        BiographyHero biography) : base(id, user, isSystem)
    {
        RequestId = request.Id;
        Request = request;
        BiographyId = biography.Id;
        Biography = biography;
    }

    /// <summary>
    /// Метод записи заявки
    /// </summary>
    /// <param name="request">Ссылка на заявку</param>
    public void SetRequest(RequestHeroRegistration request)
    {
        RequestId = request.Id;
        Request = request;
    }

    /// <summary>
    /// Метод записи биографии
    /// </summary>
    /// <param name="biography">Ссылка на биографию</param>
    public void SetBiography(BiographyHero biography)
    {
        BiographyId = biography.Id;
        Biography = biography;
    }

    /// <summary>
    /// Метод записи решения
    /// </summary>
    /// <param name="decision">Решение</param>
    /// <param name="comment">Комментарий</param>
    public void SetDecision(bool decision, string? comment)
    {
        Decision = decision;
        Comment = comment;
    }
}