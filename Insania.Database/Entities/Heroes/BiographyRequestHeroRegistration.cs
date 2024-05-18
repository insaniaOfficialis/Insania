using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

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
    [Comment("Ссылка на персонажа")]
    public long BiographyId { get; private set; }

    /// <summary>
    /// Навигационное свойство биографии персонажа
    /// </summary>
    public BiographyHero Biography { get; private set; }

    /// <summary>
    /// Решение по дате начала
    /// </summary>
    [Column("date_begin_decision")]
    [Comment("Решение по дате начала")]
    public bool? DateBeginDecision { get; private set; }

    /// <summary>
    /// Комментарий к дате начала
    /// </summary>
    [Column("comment_on_date_begin")]
    [Comment("Комментарий к дате начала")]
    public string? CommentOnDateBegin { get; private set; }

    /// <summary>
    /// Решение по дате окончания
    /// </summary>
    [Column("date_end_decision")]
    [Comment("Решение по дате окончания")]
    public bool? DateEndDecision { get; private set; }

    /// <summary>
    /// Комментарий к дате окончания
    /// </summary>
    [Column("comment_on_date_end")]
    [Comment("Комментарий к дате окончания")]
    public string? CommentOnDateEnd { get; private set; }

    /// <summary>
    /// Решение по тексту
    /// </summary>
    [Column("text_decision")]
    [Comment("Решение по тексту")]
    public bool? TextDecision { get; private set; }

    /// <summary>
    /// Комментарий к тексту
    /// </summary>
    [Column("comment_on_text")]
    [Comment("Комментарий к тексту")]
    public string? CommentOnText { get; private set; }

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
    /// Метод записи решения по дате начала
    /// </summary>
    /// <param name="dateBeginDecision">Решение по дате начала</param>
    /// <param name="commnetOnDateBegin">Комментарий к дате начала</param>
    public void SetDateBeginDecision(bool dateBeginDecision, string? commnetOnDateBegin)
    {
        DateBeginDecision = dateBeginDecision;
        CommentOnDateBegin = commnetOnDateBegin;
    }

    /// <summary>
    /// Метод записи решения по дате окончания
    /// </summary>
    /// <param name="dateBeginDecision">Решение по дате окончания</param>
    /// <param name="commnetOnDateBegin">Комментарий к дате окончания</param>
    public void SetDateEndDecision(bool dateEndDecision, string? commnetOnDateEnd)
    {
        DateEndDecision = dateEndDecision;
        CommentOnDateEnd = commnetOnDateEnd;
    }

    /// <summary>
    /// Метод записи решения по тексту
    /// </summary>
    /// <param name="textDecision">Решение по тексту</param>
    /// <param name="commnetOnText">Комментарий к тексту</param>
    public void SetTextDecision(bool textDecision, string? commnetOnText)
    {
        TextDecision = textDecision;
        CommentOnText = commnetOnText;
    }
}