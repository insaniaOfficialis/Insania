using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.Models.Heroes.Heroes;

/// <summary>
/// Модель ответа получения персонажа
/// </summary>
public class GetHeroResponse : BaseResponse
{
    /// <summary>
    /// Ссылка на игрока
    /// </summary>
    public long? PlayerId { get; set; }

    /// <summary>
    /// Личное имя
    /// </summary>
    public string? PersonalName { get; set; }

    /// <summary>
    /// Ссылка на префикс имени
    /// </summary>
    public long? PrefixNameId { get; set; }

    /// <summary>
    /// Имя семьи
    /// </summary>
    public string? FamilyName { get; set; }

    /// <summary>
    /// День рождения
    /// </summary>
    public int? BirthDay { get; set; }

    /// <summary>
    /// Ссылка на месяц рождения
    /// </summary>
    public long? BirthMonthId { get; set; }

    /// <summary>
    /// Цикл рождения
    /// </summary>
    public int? BirthCycle { get; set; }

    /// <summary>
    /// Ссылка на расу
    /// </summary>
    public long? RaceId { get; set; }

    /// <summary>
    /// Ссылка на нацию
    /// </summary>
    public long? NationId { get; set; }

    /// <summary>
    /// Пол (истина - мужской/ложь - женский)
    /// </summary>
    public bool? Gender { get; set; }

    /// <summary>
    /// Рост
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// Вес
    /// </summary>
    public int? Weight { get; set; }

    /// <summary>
    /// Ссылка на цвет волос
    /// </summary>
    public long? HairsColorId { get; set; }

    /// <summary>
    /// Ссылка на цвет глаз
    /// </summary>
    public long? EyesColorId { get; set; }

    /// <summary>
    /// Ссылка на тип телосложения
    /// </summary>
    public long? TypeBodyId { get; set; }

    /// <summary>
    /// Ссылка на тип лица
    /// </summary>
    public long? TypeFaceId { get; set; }

    /// <summary>
    /// Ссылка на текущую страну
    /// </summary>
    public long? CurrentCountryId { get; set; }

    /// <summary>
    /// Ссылка на текущий регион
    /// </summary>
    public long? CurrentRegionId { get; set; }

    /// <summary>
    /// Ссылка на текущее местоположение
    /// </summary>
    public long? CurrentLocationId { get; set; }

    /// <summary>
    /// Ссылка на файл
    /// </summary>
    public long? FileId { get; set; }

    /// <summary>
    /// Простой конструктор модели ответа получения персонажа
    /// </summary>
    public GetHeroResponse() : base()
    {

    }

    /// <summary>
    /// Конструктор модели ответа получения персонажа
    /// </summary>
    /// <param name="success">Признак успешности</param>
    /// <param name="id">Первичный ключ сущности</param>
    /// <param name="playerId">Игрок</param>
    /// <param name="personalName">Личное имя</param>
    /// <param name="prefixNameId">Префикс имени</param>
    /// <param name="familyName">Имя семьи</param>
    /// <param name="birthDay">День рождения</param>
    /// <param name="birthMonthId">Месяц рождения</param>
    /// <param name="birthCycle">Цикл рождения</param>
    /// <param name="raceId">Раса</param>
    /// <param name="nationId">Нация</param>
    /// <param name="gender">Пол</param>
    /// <param name="height">Рост</param>
    /// <param name="weight">Вес</param>
    /// <param name="hairsColorId">Цвет волос</param>
    /// <param name="eyesColorId">Цвет глаз</param>
    /// <param name="typeBodyId">Тип телосложения</param>
    /// <param name="typeFaceId">Тип лица</param>
    /// <param name="currentCountryId">Текущая страна</param>
    /// <param name="currentRegionId">Текущий регион</param>
    /// <param name="currentLocationId">Текущая область</param>
    /// <param name="fileId">Файл</param>
    /// <exception cref="Exception">Необработанное исключение</exception>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    public GetHeroResponse(bool success, long id, long? playerId, string? personalName, long? prefixNameId, string? familyName,
        int? birthDay, long? birthMonthId, int? birthCycle, long? raceId, long? nationId, bool? gender, int? height, int? weight,
        long? hairsColorId, long? eyesColorId, long? typeBodyId, long? typeFaceId, long? currentCountryId, long? currentRegionId,
        long? currentLocationId, long? fileId) : base(success, id)
    {
        //Проверяем входные данные
        if (id == 0) throw new InnerException(Errors.EmptyId);
        if ((playerId ?? 0) == 0) throw new InnerException(Errors.EmptyPlayer);
        if (string.IsNullOrWhiteSpace(personalName)) throw new InnerException(Errors.EmptyPersonalName);
        if ((birthDay ?? 0) == 0 || (birthMonthId ?? 0) == 0 || (birthCycle ?? 0) == 0) throw new InnerException(Errors.EmptyBirthDate);
        if (birthDay < 1 || birthDay > 30) throw new InnerException(Errors.IncorrectBirthDate);
        if ((raceId ?? 0) == 0) throw new InnerException(Errors.EmptyRace);
        if ((nationId ?? 0) == 0) throw new InnerException(Errors.EmptyNation);
        if ((height ?? 0) <= 0) throw new InnerException(Errors.EmptyHeight);
        if ((weight ?? 0) <= 0) throw new InnerException(Errors.EmptyWeight);
        if ((eyesColorId ?? 0) == 0) throw new InnerException(Errors.EmptyEyesColor);
        if ((typeBodyId ?? 0) == 0) throw new InnerException(Errors.EmptyTypeBody);
        if ((typeFaceId ?? 0) == 0) throw new InnerException(Errors.EmptyTypeFace);
        if ((currentCountryId ?? 0) == 0) throw new InnerException(Errors.EmptyCountry);
        if ((currentRegionId ?? 0) == 0) throw new InnerException(Errors.EmptyRegion);
        if ((currentLocationId ?? 0) == 0) throw new InnerException(Errors.EmptyArea);
        if ((fileId ?? 0) == 0) throw new InnerException(Errors.EmptyFile);

        //Заполняем модель
        PlayerId = playerId;
        PersonalName = personalName;
        PrefixNameId = prefixNameId;
        FamilyName = familyName;
        BirthDay = birthDay;
        BirthMonthId = birthMonthId;
        BirthCycle = birthCycle;
        RaceId = raceId;
        NationId = nationId;
        Gender = gender ?? throw new InnerException(Errors.EmptyGender);
        Height = height;
        Weight = weight;
        HairsColorId = hairsColorId;
        EyesColorId = eyesColorId;
        TypeBodyId = typeBodyId;
        TypeFaceId = typeFaceId;
        CurrentCountryId = currentCountryId;
        CurrentRegionId = currentRegionId;
        CurrentLocationId = currentLocationId;
        FileId = fileId;
    }

}