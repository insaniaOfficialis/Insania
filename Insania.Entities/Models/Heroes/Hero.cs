using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Models.Appearance;
using Insania.Entities.Base;
using Insania.Entities.Models.Players;
using Insania.Entities.Models.Biology;
using Insania.Entities.Models.Chronology;

namespace Insania.Entities.Models.Heroes;

/// <summary>
/// Модель сущности персонажа
/// </summary>
[Table("re_heroes")]
[Comment("Персонажи")]
public class Hero : Reestr
{
    /// <summary>
    /// Ссылка на игрока
    /// </summary>
    [Column("player_id")]
    [Comment("Ссылка на игрока")]
    public long PlayerId { get; private set; }

    /// <summary>
    /// Навигационное свойство игрока
    /// </summary>
    public Player Player { get; private set; }

    /// <summary>
    /// Личное имя
    /// </summary>
    [Column("personal_name")]
    [Comment("Личное имя")]
    public string PersonalName { get; private set; }

    /// <summary>
    /// Префикс имени
    /// </summary>
    [Column("prefix_name")]
    [Comment("Префикс имени")]
    public string? PrefixName { get; private set; }

    /// <summary>
    /// Имя семьи
    /// </summary>
    [Column("family_name")]
    [Comment("Имя семьи")]
    public string? FamilyName { get; private set; }

    /// <summary>
    /// День рождения
    /// </summary>
    [Column("birth_day")]
    [Comment("День рождения")]
    public int BirthDay { get; private set; }

    /// <summary>
    /// Ссылка на месяц рождения
    /// </summary>
    [Column("birth_month_id")]
    [Comment("Ссылка на месяц рождения")]
    public long BirthMonthId { get; private set; }

    /// <summary>
    /// Навигационное свойство месяца рождения
    /// </summary>
    public Month BirthMonth { get; private set; }

    /// <summary>
    /// Цикл рождения
    /// </summary>
    [Column("birth_cycle")]
    [Comment("Цикл рождения")]
    public int BirthCycle { get; private set; }

    /// <summary>
    /// Ссылка на нацию
    /// </summary>
    [Column("nation_id")]
    [Comment("Ссылка на нацию")]
    public long NationId { get; private set; }

    /// <summary>
    /// Навигационное свойство нации
    /// </summary>
    public Nation Nation { get; private set; }

    /// <summary>
    /// Пол (истина - мужской/ложь - женский)
    /// </summary>
    [Comment("Пол (истина - мужской/ложь - женский)")]
    public bool Gender { get; private set; }

    /// <summary>
    /// Рост
    /// </summary>
    [Column("height")]
    [Comment("Рост")]
    public int Height { get; private set; }

    /// <summary>
    /// Вес
    /// </summary>
    [Column("weight")]
    [Comment("Вес")]
    public int Weight { get; private set; }

    /// <summary>
    /// Ссылка на цвет волос
    /// </summary>
    [Column("hair_color_id")]
    [Comment("Ссылка на цвет волос")]
    public long HairColorId { get; private set; }

    /// <summary>
    /// Навигационное свойство цвета волос
    /// </summary>
    public HairsColor HairsColor { get; private set; }

    /// <summary>
    /// Ссылка на цвет глаз
    /// </summary>
    [Column("eye_color_id")]
    [Comment("Ссылка на цвет глаз")]
    public long EyesColorId { get; private set; }

    /// <summary>
    /// Навигационное свойство цвета глаз
    /// </summary>
    public EyesColor EyesColor { get; private set; }

    /// <summary>
    /// Ссылка на тип телосложения
    /// </summary>
    [Column("type_body_id")]
    [Comment("Ссылка на тип телосложения")]
    public long TypeBodyId { get; private set; }

    /// <summary>
    /// Навигационное свойство типа телосложения
    /// </summary>
    public TypeBody TypeBody { get; private set; }

    /// <summary>
    /// Ссылка на тип лица
    /// </summary>
    [Column("type_face_id")]
    [Comment("Ссылка на тип лица")]
    public long TypeFaceId { get; private set; }

    /// <summary>
    /// Навигационное свойство типа лица
    /// </summary>
    public TypeFace TypeFace { get; private set; }

    /// <summary>
    /// Признак активности
    /// </summary>
    [Column("is_active")]
    [Comment("Признак активности")]
    [DefaultValue(true)]
    public bool IsActive { get; private set; }

    /// <summary>
    /// Признак текущего
    /// </summary>
    [Column("is_current")]
    [Comment("Признак текущего")]
    public bool IsCurrent { get; private set; }

    /// <summary>
    /// Заморозка да
    /// </summary>
    [Column("freezing_to")]
    [Comment("Заморозка да")]
    public DateTime? FreezingTo { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности персонажа
    /// </summary>
    public Hero() : base()
    {
        Player = new();
        PersonalName = string.Empty;
        BirthMonth = new();
        Nation = new();
        HairsColor = new();
        EyesColor = new();
        TypeBody = new();
        TypeFace = new();
    }

    /// <summary>
    /// Конструктор модели сущности персонажа без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="player">Игрок</param>
    /// <param name="personalName">Личное имя</param>
    /// <param name="prefixName">Префикс имени</param>
    /// <param name="familyName">Имя семьи</param>
    /// <param name="birthDay">День рождения</param>
    /// <param name="birthMonth">Месяц рождения</param>
    /// <param name="birthCycle">Цикл рождения</param>
    /// <param name="nation">Нация</param>
    /// <param name="gender">Пол</param>
    /// <param name="height">Рост</param>
    /// <param name="weight">Вес</param>
    /// <param name="hairsColor">Цвет волос</param>
    /// <param name="eyesColor">Цвет глаз</param>
    /// <param name="typeBody">Тип телосложения</param>
    /// <param name="typeFace">Тип лица</param>
    /// <param name="isActive">Признак активности</param>
    /// <param name="isCurrent">Признак текущего</param>
    /// <param name="freezingTo">Заморозка до</param>
    public Hero(string user, bool isSystem, Player player, string personalName, string? prefixName,
        string? familyName, int birthDay, Month birthMonth, int birthCycle, Nation nation, bool gender, int height,
        int weight, HairsColor hairsColor, EyesColor eyesColor, TypeBody typeBody, TypeFace typeFace, bool isActive,
        bool isCurrent, DateTime? freezingTo) : base(user, isSystem)
    {
        PlayerId = player.Id;
        Player = player;
        PersonalName = personalName;
        PrefixName = prefixName;
        FamilyName = familyName;
        BirthDay = birthDay;
        BirthMonthId = birthMonth.Id;
        BirthMonth = birthMonth;
        BirthCycle = birthCycle;
        NationId = nation.Id;
        Nation = nation;
        Gender = gender;
        Height = height;
        Weight = weight;
        HairColorId = hairsColor.Id;
        HairsColor = hairsColor;
        EyesColorId = eyesColor.Id;
        EyesColor = eyesColor;
        TypeBodyId = typeBody.Id;
        TypeBody = typeBody;
        TypeFaceId = typeFace.Id;
        TypeFace = typeFace;
        IsActive = isActive;
        IsCurrent = isCurrent;
        FreezingTo = freezingTo;
    }

    /// <summary>
    /// Конструктор модели сущности персонажа c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="player">Игрок</param>
    /// <param name="personalName">Личное имя</param>
    /// <param name="prefixName">Префикс имени</param>
    /// <param name="familyName">Имя семьи</param>
    /// <param name="birthDay">День рождения</param>
    /// <param name="birthMonth">Месяц рождения</param>
    /// <param name="birthCycle">Цикл рождения</param>
    /// <param name="nation">Нация</param>
    /// <param name="gender">Пол</param>
    /// <param name="height">Рост</param>
    /// <param name="weight">Вес</param>
    /// <param name="hairsColor">Цвет волос</param>
    /// <param name="eyesColor">Цвет глаз</param>
    /// <param name="typeBody">Тип телосложения</param>
    /// <param name="typeFace">Тип лица</param>
    /// <param name="isActive">Признак активности</param>
    /// <param name="isCurrent">Признак текущего</param>
    /// <param name="freezingTo">Заморозка до</param>
    public Hero(long id, string user, bool isSystem, Player player, string personalName, string? prefixName,
        string? familyName, int birthDay, Month birthMonth, int birthCycle, Nation nation, bool gender, int height,
        int weight, HairsColor hairsColor, EyesColor eyesColor, TypeBody typeBody, TypeFace typeFace, bool isActive,
        bool isCurrent, DateTime? freezingTo) : base(id, user, isSystem)
    {
        PlayerId = player.Id;
        Player = player;
        PersonalName = personalName;
        PrefixName = prefixName;
        FamilyName = familyName;
        BirthDay = birthDay;
        BirthMonthId = birthMonth.Id;
        BirthMonth = birthMonth;
        BirthCycle = birthCycle;
        NationId = nation.Id;
        Nation = nation;
        Gender = gender;
        Height = height;
        Weight = weight;
        HairColorId = hairsColor.Id;
        HairsColor = hairsColor;
        EyesColorId = eyesColor.Id;
        EyesColor = eyesColor;
        TypeBodyId = typeBody.Id;
        TypeBody = typeBody;
        TypeFaceId = typeFace.Id;
        TypeFace = typeFace;
        IsActive = isActive;
        IsCurrent = isCurrent;
        FreezingTo = freezingTo;
    }

    /// <summary>
    /// Метод записи игрока
    /// </summary>
    /// <param name="player">Игрок</param>
    public void SetPlayer(Player player)
    {
        PlayerId = player.Id;
        Player = player;
    }

    /// <summary>
    /// Метод записи личного имени
    /// </summary>
    /// <param name="personalName">Личное имя</param>
    public void SetPersonalName(string personalName)
    {
        PersonalName = personalName;
    }

    /// <summary>
    /// Метод записи префикса имени
    /// </summary>
    /// <param name="prefixName">Префикс имени</param>
    public void SetPrefixName(string prefixName)
    {
        PrefixName = prefixName;
    }

    /// <summary>
    /// Метод записи имени семьи
    /// </summary>
    /// <param name="familyName">Имя семьи</param>
    public void SetFamilyName(string familyName)
    {
        FamilyName = familyName;
    }

    /// <summary>
    /// Метод записи дня рождения
    /// </summary>
    /// <param name="birthDay">День рождения</param>
    public void SetBirthDay(int birthDay)
    {
        BirthDay = birthDay;
    }

    /// <summary>
    /// Метод записи месяца рождения
    /// </summary>
    /// <param name="birthMonth">Месяц рождения</param>
    public void SetBirthMonth(Month birthMonth)
    {
        BirthMonthId = birthMonth.Id;
        BirthMonth = birthMonth;
    }

    /// <summary>
    /// Метод записи цикла рождения
    /// </summary>
    /// <param name="birthCycle">Цикл рождения</param>
    public void SetBirthCycle(int birthCycle)
    {
        BirthCycle = birthCycle;
    }

    /// <summary>
    /// Метод записи нации
    /// </summary>
    /// <param name="nation">Нация</param>
    public void SetNation(Nation nation)
    {
        NationId = nation.Id;
        Nation = nation;
    }

    /// <summary>
    /// Метод записи пола
    /// </summary>
    /// <param name="gender">Пол</param>
    public void SetGender(bool gender)
    {
        Gender = gender;
    }

    /// <summary>
    /// Метод записи роста
    /// </summary>
    /// <param name="height">Рост</param>
    public void SetHeight(int height)
    {
        Height = height;
    }

    /// <summary>
    /// Метод записи веса
    /// </summary>
    /// <param name="weight">Вес</param>
    public void SetWeight(int weight)
    {
        Weight = weight;
    }

    /// <summary>
    /// Метод записи цвета волос
    /// </summary>
    /// <param name="hairsColor">Цвет волос</param>
    public void SetHairColor(HairsColor hairsColor)
    {
        HairColorId = hairsColor.Id;
        HairsColor = hairsColor;
    }

    /// <summary>
    /// Метод записи цвета глаз
    /// </summary>
    /// <param name="eyesColor">Цвет глаз</param>
    public void SetEyesColor(EyesColor eyesColor)
    {
        EyesColorId = eyesColor.Id;
        EyesColor = eyesColor;
    }

    /// <summary>
    /// Метод записи типа телосложения
    /// </summary>
    /// <param name="typeBody">Тип телосложения</param>
    public void SetTypeBody(TypeBody typeBody)
    {
        TypeBodyId = typeBody.Id;
        TypeBody = typeBody;
    }

    /// <summary>
    /// Метод записи типа лица
    /// </summary>
    /// <param name="typeFace">Тип лица</param>
    public void SetTypeFace(TypeFace typeFace)
    {
        TypeFaceId = typeFace.Id;
        TypeFace = typeFace;
    }

    /// <summary>
    /// Метод записи признака активности
    /// </summary>
    /// <param name="isActive">Признак активности</param>
    public void SetIsActive(bool isActive)
    {
        IsActive = isActive;
    }

    /// <summary>
    /// Метод записи признака текущего
    /// </summary>
    /// <param name="isCurrent">Признак текущего</param>
    public void SetIsCurrent(bool isCurrent)
    {
        IsCurrent = isCurrent;
    }

    /// <summary>
    /// Метод записи заморозки до
    /// </summary>
    /// <param name="freezingTo">Заморозка до</param>
    public void SetFreezingTo(DateTime freezingTo)
    {
        FreezingTo = freezingTo;
    }
}