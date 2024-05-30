namespace Insania.Models.Heroes.Heroes;

/// <summary>
/// Модель запроса добавления персонажа
/// </summary>
public class AddHeroRequest
{
    /// <summary>
    /// Ссылка на игрока
    /// </summary>
    public long? PlayerId { get; private set; }

    /// <summary>
    /// Личное имя
    /// </summary>
    public string? PersonalName { get; private set; }

    /// <summary>
    /// Префикс имени
    /// </summary>
    public string? PrefixName { get; private set; }

    /// <summary>
    /// Имя семьи
    /// </summary>
    public string? FamilyName { get; private set; }

    /// <summary>
    /// День рождения
    /// </summary>
    public int? BirthDay { get; private set; }

    /// <summary>
    /// Ссылка на месяц рождения
    /// </summary>
    public long? BirthMonthId { get; private set; }

    /// <summary>
    /// Цикл рождения
    /// </summary>
    public int? BirthCycle { get; private set; }

    /// <summary>
    /// Ссылка на нацию
    /// </summary>
    public long? NationId { get; private set; }

    /// <summary>
    /// Пол (истина - мужской/ложь - женский)
    /// </summary>
    public bool? Gender { get; private set; }

    /// <summary>
    /// Рост
    /// </summary>
    public int? Height { get; private set; }

    /// <summary>
    /// Вес
    /// </summary>
    public int? Weight { get; private set; }

    /// <summary>
    /// Ссылка на цвет волос
    /// </summary>
    public long? HairsColorId { get; private set; }

    /// <summary>
    /// Ссылка на цвет глаз
    /// </summary>
    public long? EyesColorId { get; private set; }

    /// <summary>
    /// Ссылка на тип телосложения
    /// </summary>
    public long? TypeBodyId { get; private set; }

    /// <summary>
    /// Ссылка на тип лица
    /// </summary>
    public long? TypeFaceId { get; private set; }

    /// <summary>
    /// Ссылка на текущее местоположение
    /// </summary>
    public long? CurrentLocationId { get; private set; }

    /// <summary>
    /// Простой конструктор модели запроса добавления персонажа
    /// </summary>
    public AddHeroRequest()
    {
        
    }

    /// <summary>
    /// Конструктор модели запроса добавления персонажа
    /// </summary>
    /// <param name="playerId">Игрок</param>
    /// <param name="personalName">Личное имя</param>
    /// <param name="prefixName">Префикс имени</param>
    /// <param name="familyName">Имя семьи</param>
    /// <param name="birthDay">День рождения</param>
    /// <param name="birthMonthId">Месяц рождения</param>
    /// <param name="birthCycle">Цикл рождения</param>
    /// <param name="nationId">Нация</param>
    /// <param name="gender">Пол (истина - мужской/ложь - женский)</param>
    /// <param name="height">Рост</param>
    /// <param name="weight">Вес</param>
    /// <param name="hairsColorId">Цвет волос</param>
    /// <param name="eyesColorId">Цвет глаз</param>
    /// <param name="typeBodyId">Тип телосложения</param>
    /// <param name="typeFaceId">Тип лица</param>
    /// <param name="currentLocationId">Текущая локация</param>
    public AddHeroRequest(long? playerId, string? personalName, string? prefixName, string? familyName, int? birthDay,
        long? birthMonthId, int? birthCycle, long? nationId, bool? gender, int? height, int? weight, long? hairsColorId,
        long eyesColorId, long? typeBodyId, long? typeFaceId, long? currentLocationId)
    {


        PlayerId = playerId;
        PersonalName = personalName;
        PrefixName = prefixName;
        FamilyName = familyName;
        BirthDay = birthDay;
        BirthMonthId = birthMonthId;
        BirthCycle = birthCycle;
        NationId = nationId;
        Gender = gender;
        Height = height;
        Weight = weight;
        HairsColorId = hairsColorId;
        EyesColorId = eyesColorId;
        TypeBodyId = typeBodyId;
        TypeFaceId = typeFaceId;
        CurrentLocationId = currentLocationId;
    }
}