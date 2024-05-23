using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Insania.Entities.Base;
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
    /// Решение по личному имени
    /// </summary>
    [Column("personal_name_decision")]
    [Comment("Решение по личному имени")]
    public bool? PersonalNameDecision { get; private set; }

    /// <summary>
    /// Комментарий к личному имени
    /// </summary>
    [Column("comment_on_personal_name")]
    [Comment("Комментарий к личному имени")]
    public string? CommentOnPersonalName { get; private set; }

    /// <summary>
    /// Решение по имени семьи
    /// </summary>
    [Column("family_name_decision")]
    [Comment("Решение по имени семьи")]
    public bool? FamilyNameDecision { get; private set; }

    /// <summary>
    /// Комментарий к имени семьи
    /// </summary>
    [Column("comment_on_family_name")]
    [Comment("Комментарий к имени семьи")]
    public string? CommentOnFamilyName { get; private set; }

    /// <summary>
    /// Решение по расе
    /// </summary>
    [Column("race_decision")]
    [Comment("Решение по расе")]
    public bool? RaceDecision { get; private set; }

    /// <summary>
    /// Комментарий к расе
    /// </summary>
    [Column("comment_on_race")]
    [Comment("Комментарий к расе")]
    public string? CommentOnRace { get; private set; }

    /// <summary>
    /// Решение по нации
    /// </summary>
    [Column("nation_decision")]
    [Comment("Решение по нации")]
    public bool? NationDecision { get; private set; }

    /// <summary>
    /// Комментарий к нации
    /// </summary>
    [Column("comment_on_nation")]
    [Comment("Комментарий к нации")]
    public string? CommentOnNation { get; private set; }

    /// <summary>
    /// Решение по дате рождения
    /// </summary>
    [Column("birth_date_decision")]
    [Comment("Решение по дате рождения")]
    public bool? BirthDateDecision { get; private set; }

    /// <summary>
    /// Комментарий к дате рождения
    /// </summary>
    [Column("comment_on_birth_date")]
    [Comment("Комментарий к дате рождения")]
    public string? CommentOnBirthDate { get; private set; }

    /// <summary>
    /// Решение по местоположению
    /// </summary>
    [Column("location_decision")]
    [Comment("Решение по местоположению")]
    public bool? LocationDecision { get; private set; }

    /// <summary>
    /// Комментарий к местоположению
    /// </summary>
    [Column("comment_on_location")]
    [Comment("Комментарий к местоположению")]
    public string? CommentOnLocation { get; private set; }

    /// <summary>
    /// Решение по росту
    /// </summary>
    [Column("height_decision")]
    [Comment("Решение по росту")]
    public bool? HeightDecision { get; private set; }

    /// <summary>
    /// Комментарий к росту
    /// </summary>
    [Column("comment_on_height")]
    [Comment("Комментарий к росту")]
    public string? CommentOnHeight { get; private set; }

    /// <summary>
    /// Решение по весу
    /// </summary>
    [Column("weight_decision")]
    [Comment("Решение по весу")]
    public bool? WeightDecision { get; private set; }

    /// <summary>
    /// Комментарий к весу
    /// </summary>
    [Column("comment_on_weight")]
    [Comment("Комментарий к весу")]
    public string? CommentOnWeight { get; private set; }

    /// <summary>
    /// Решение по типу телосложения
    /// </summary>
    [Column("type_body_decision")]
    [Comment("Решение по типу телосложения")]
    public bool? TypeBodyDecision { get; private set; }

    /// <summary>
    /// Комментарий к типу телосложения
    /// </summary>
    [Column("comment_on_type_body")]
    [Comment("Комментарий к типу телосложения")]
    public string? CommentOnTypeBody { get; private set; }

    /// <summary>
    /// Решение по типу лица
    /// </summary>
    [Column("type_face_decision")]
    [Comment("Решение по типу лица")]
    public bool? TypeFaceDecision { get; private set; }

    /// <summary>
    /// Комментарий к типу лица
    /// </summary>
    [Column("comment_on_type_face")]
    [Comment("Комментарий к типу лица")]
    public string? CommentOnTypeFace { get; private set; }

    /// <summary>
    /// Решение по цвету волос
    /// </summary>
    [Column("hair_color_decision")]
    [Comment("Решение по цвету волос")]
    public bool? HairColorDecision { get; private set; }

    /// <summary>
    /// Комментарий к цвету волос
    /// </summary>
    [Column("comment_on_hair_color")]
    [Comment("Комментарий к цвету волос")]
    public string? CommentOnHairColor { get; private set; }

    /// <summary>
    /// Решение по цвету глаз
    /// </summary>
    [Column("eye_color_decision")]
    [Comment("Решение по цвету глаз")]
    public bool? EyesColorDecision { get; private set; }

    /// <summary>
    /// Комментарий к цвету глаз
    /// </summary>
    [Column("comment_on_eye_color")]
    [Comment("Комментарий к цвету глаз")]
    public string? CommentOnEyesColor { get; private set; }

    /// <summary>
    /// Решение по изображению
    /// </summary>
    [Column("image_decision")]
    [Comment("Решение по изображению")]
    public bool? ImageDecision { get; private set; }

    /// <summary>
    /// Комментарий к изображению
    /// </summary>
    [Column("comment_on_image")]
    [Comment("Комментарий к изображению")]
    public string? CommentOnImage { get; private set; }

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
    /// Метод записи решения по персональному имени
    /// </summary>
    /// <param name="personalNameDecision">Решение по личному имени</param>
    /// <param name="commentOnPersonalName">Комментарий к личному имени</param>
    public void SetPersonalNameDecision(bool personalNameDecision,
        string? commentOnPersonalName)
    {
        PersonalNameDecision = personalNameDecision;
        CommentOnPersonalName = commentOnPersonalName;
    }

    /// <summary>
    /// Метод записи решения по имени семьи
    /// </summary>
    /// <param name="familyNameDecision">Решение по имени семьи</param>
    /// <param name="commentOnFamilyName">Комментарий к имени семьи</param>
    public void SetFamilyNameDecision(bool familyNameDecision,
        string? commentOnFamilyName)
    {
        FamilyNameDecision = familyNameDecision;
        CommentOnFamilyName = commentOnFamilyName;
    }

    /// <summary>
    /// Метод записи решения по расе
    /// </summary>
    /// <param name="raceDecision">Решение по расе</param>
    /// <param name="commentOnRace">Комментарий к расе</param>
    public void SetRaceDecision(bool raceDecision, string? commentOnRace)
    {
        RaceDecision = raceDecision;
        CommentOnRace = commentOnRace;
    }

    /// <summary>
    /// Метод записи решения по нации
    /// </summary>
    /// <param name="nationDecision">Решение по нации</param>
    /// <param name="commentOnNation">Комментарий к нации</param>
    public void SetNationDecision(bool nationDecision, string? commentOnNation)
    {
        NationDecision = nationDecision;
        CommentOnNation = commentOnNation;
    }

    /// <summary>
    /// Метод записи решения по местоположению
    /// </summary>
    /// <param name="locationDecision">Решение по местоположению</param>
    /// <param name="commentOnLocation">Комментарий к местоположению</param>
    public void SetLocationDecision(bool locationDecision, string? commentOnLocation)
    {
        LocationDecision = locationDecision;
        CommentOnLocation = commentOnLocation;
    }

    /// <summary>
    /// Метод записи решения по росту
    /// </summary>
    /// <param name="heightDecision">Решение по росту</param>
    /// <param name="commentOnHeight">Комментарий к росту</param>
    public void SetHeightDecision(bool heightDecision, string? commentOnHeight)
    {
        HeightDecision = heightDecision;
        CommentOnHeight = commentOnHeight;
    }

    /// <summary>
    /// Метод записи решения по весу
    /// </summary>
    /// <param name="weightDecision">Решение по весу</param>
    /// <param name="commentOnWeight">Комментарий к весу</param>
    public void SetWeightDecision(bool weightDecision, string? commentOnWeight)
    {
        WeightDecision = weightDecision;
        CommentOnWeight = commentOnWeight;
    }

    /// <summary>
    /// Метод записи решения по типу телосложения
    /// </summary>
    /// <param name="typeBodyDecision">Решение по типу телосложения</param>
    /// <param name="commentOnTypeBody">Комментарий к типу телосложения</param>
    public void SetTypeBodyDescision(bool typeBodyDecision, string? commentOnTypeBody)
    {
        TypeBodyDecision = typeBodyDecision;
        CommentOnTypeBody = commentOnTypeBody;
    }

    /// <summary>
    /// Метод записи решения по типу лица
    /// </summary>
    /// <param name="typeFaceDecision">Решение по типу лица</param>
    /// <param name="commentOnTypeFace">Комментарий к типу лица</param>
    public void SetTypeFaceDescision(bool typeFaceDecision, string? commentOnTypeFace)
    {
        TypeFaceDecision = typeFaceDecision;
        CommentOnTypeFace = commentOnTypeFace;
    }

    /// <summary>
    /// Метод записи решения по цвету волос
    /// </summary>
    /// <param name="hairColorDecision">Решение по цвету волос</param>
    /// <param name="comentOnHairColor">Комментарий к цвету волос</param>
    public void SetHairColorDescision(bool hairColorDecision, string? comentOnHairColor)
    {
        HairColorDecision = hairColorDecision;
        CommentOnHairColor = comentOnHairColor;
    }

    /// <summary>
    /// Метод записи решения по цвету глаз
    /// </summary>
    /// <param name="eyeColorDecision">Решение по цвету глаз</param>
    /// <param name="comentOnEyesColor">Комментарий к цвету глаз</param>
    public void SetEyesColor(bool eyeColorDecision, string? comentOnEyesColor)
    {
        EyesColorDecision = eyeColorDecision;
        CommentOnEyesColor = comentOnEyesColor;
    }

    /// <summary>
    /// Метод записи решения по изображению
    /// </summary>
    /// <param name="imageDecision">Решение по изображению</param>
    /// <param name="commentOnImage">Комментарий к изображению</param>
    public void SetImageDecision(bool imageDecision, string? commentOnImage)
    {
        ImageDecision = imageDecision;
        CommentOnImage = commentOnImage;
    }
}