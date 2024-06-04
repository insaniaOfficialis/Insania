using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Insania.Entities.OutCategories;
using Insania.Database.Entities.Administrators;

namespace Insania.Database.Entities.Heroes;

/// <summary>
/// Модель сущности заявки на регистрацию персонажа
/// </summary>
[Table("re_requests_heroes_registration")]
[Comment("Заявки на регистрацию персонажей")]
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
    public StatusRequestHeroRegistration Status { get; private set; }

    /// <summary>
    /// Ссылка на ответственного администратора
    /// </summary>
    [Column("administrator_id")]
    [Comment("Ссылка на ответственного администратора")]
    public long? AdministratorId { get; private set; }

    /// <summary>
    /// Навигационное свойство ответственного администратора
    /// </summary>
    public Administrator? Administrator { get; private set; }

    /// <summary>
    /// Решение по блоку общее
    /// </summary>
    [Column("general_block_decision")]
    [Comment("Решение по блоку общее")]
    public bool? GeneralBlockDecision { get; private set; }

    /// <summary>
    /// Комментарий к блоку общее
    /// </summary>
    [Column("comment_on_general_block")]
    [Comment("Комментарий к блоку общее")]
    public string? CommentOnGeneralBlock { get; private set; }

    /// <summary>
    /// Решение по блоку дата рождения
    /// </summary>
    [Column("birth_date_block_decision")]
    [Comment("Решение по блоку дата рождения")]
    public bool? BirthDateBlockDecision { get; private set; }

    /// <summary>
    /// Комментарий к блоку дата рождения
    /// </summary>
    [Column("comment_on_birth_date_block")]
    [Comment("Комментарий к блоку дата рождения")]
    public string? CommentOnBirthDateBlock { get; private set; }

    /// <summary>
    /// Решение по блоку местоположение
    /// </summary>
    [Column("location_block_decision")]
    [Comment("Решение по блоку местоположение")]
    public bool? LocationBlockDecision { get; private set; }

    /// <summary>
    /// Комментарий к блоку местоположение
    /// </summary>
    [Column("comment_on_location_block")]
    [Comment("Комментарий к блоку местоположение")]
    public string? CommentOnLocationBlock { get; private set; }

    /// <summary>
    /// Решение по блоку внешность
    /// </summary>
    [Column("appearance_block_decision")]
    [Comment("Решение по блоку внешность")]
    public bool? AppearanceBlockDecision { get; private set; }

    /// <summary>
    /// Комментарий к блоку внешность
    /// </summary>
    [Column("comment_on_appearance_block")]
    [Comment("Комментарий к блоку внешность")]
    public string? CommentOnAppearanceBlock { get; private set; }

    /// <summary>
    /// Решение по блоку изображение
    /// </summary>
    [Column("image_block_decision")]
    [Comment("Решение по блоку изображение")]
    public bool? ImageBlockDecision { get; private set; }

    /// <summary>
    /// Комментарий к блоку изображение
    /// </summary>
    [Column("comment_on_image_block")]
    [Comment("Комментарий к блоку изображение")]
    public string? CommentOnImageBlock { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности заявки на регистрацию персонажа
    /// </summary>
    public RequestHeroRegistration() : base()
    {
        Hero = new();
        Status = new();
    }

    /// <summary>
    /// Конструктор модели сущности заявки на регистрацию персонажа без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="hero">Ссылка на персонажа</param>
    /// <param name="status">Ссылка на статус</param>
    public RequestHeroRegistration(string user, bool isSystem, Hero hero, StatusRequestHeroRegistration status) : base(user, 
        isSystem)
    {
        HeroId = hero.Id;
        Hero = hero;
        StatusId = status.Id;
        Status = status;
    }

    /// <summary>
    /// Конструктор модели сущности заявки на регистрацию персонажа c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="hero">Ссылка на персонажа</param>
    /// <param name="status">Ссылка на статус</param>
    public RequestHeroRegistration(long id, string user, bool isSystem, Hero hero, StatusRequestHeroRegistration status) : 
        base(id, user, isSystem)
    {
        HeroId = hero.Id;
        Hero = hero;
        StatusId = status.Id;
        Status = status;
    }

    /// <summary>
    /// Метод записи персонажа
    /// </summary>
    /// <param name="hero">Ссылка на персонажа</param>
    public void SetHero(Hero hero)
    {
        HeroId = hero.Id;
        Hero = hero;
    }

    /// <summary>
    /// Метод записи статуса
    /// </summary>
    /// <param name="status">Ссылка на статус</param>
    public void SetStatus(StatusRequestHeroRegistration status)
    {
        StatusId = status.Id;
        Status = status;
    }

    /// <summary>
    /// Метод записи ответственного администратора
    /// </summary>
    /// <param name="administrator">Ссылка на ответственного администратора</param>
    public void SetAdministrator(Administrator administrator)
    {
        AdministratorId = administrator.Id;
        Administrator = administrator;
    }

    /// <summary>
    /// Метод записи решения по блоку общее
    /// </summary>
    /// <param name="decision">Решение</param>
    /// <param name="decision">Комментарий</param>
    public void SetGeneralBlockDecision(bool decision, string? comment)
    {
        GeneralBlockDecision = decision;
        CommentOnGeneralBlock = comment;
    }

    /// <summary>
    /// Метод записи решения по блоку дата рождения
    /// </summary>
    /// <param name="decision">Решение</param>
    /// <param name="decision">Комментарий</param>
    public void SetBirthDateBlockDecision(bool decision, string? comment)
    {
        BirthDateBlockDecision = decision;
        CommentOnBirthDateBlock = comment;
    }

    /// <summary>
    /// Метод записи решения по блоку местоположение
    /// </summary>
    /// <param name="decision">Решение</param>
    /// <param name="decision">Комментарий</param>
    public void SetLocationBlockDecision(bool decision, string? comment)
    {
        LocationBlockDecision = decision;
        CommentOnLocationBlock = comment;
    }

    /// <summary>
    /// Метод записи решения по блоку местоположение
    /// </summary>
    /// <param name="decision">Решение</param>
    /// <param name="decision">Комментарий</param>
    public void SetAppearanceBlockDecision(bool decision, string? comment)
    {
        AppearanceBlockDecision = decision;
        CommentOnAppearanceBlock = comment;
    }

    /// <summary>
    /// Метод записи решения по блоку изображение
    /// </summary>
    /// <param name="decision">Решение</param>
    /// <param name="decision">Комментарий</param>
    public void SetImageBlockDecision(bool decision, string? comment)
    {
        ImageBlockDecision = decision;
        CommentOnImageBlock = comment;
    }
}