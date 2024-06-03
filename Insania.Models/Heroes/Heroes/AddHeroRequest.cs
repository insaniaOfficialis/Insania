using Insania.Models.Heroes.BiographiesHeroes;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Models.Users.Users;

namespace Insania.Models.Heroes.Heroes;

/// <summary>
/// Модель запроса добавления персонажа
/// </summary>
public class AddHeroRequest
{
    /// <summary>
    /// Ссылка на игрока
    /// </summary>
    public long? PlayerId { get; set; }

    /// <summary>
    /// Анкета пользователя
    /// </summary>
    public AddUserRequest? User { get; set; }

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
    /// Ссылка на текущее местоположение
    /// </summary>
    public long? CurrentLocationId { get; set; }

    /// <summary>
    /// Биографии
    /// </summary>
    public List<AddBiographyHeroRequest>? Biographies {  get; set; }

    /// <summary>
    /// Простой конструктор модели запроса добавления персонажа
    /// </summary>
    public AddHeroRequest()
    {
        
    }

    /// <summary>
    /// Конструктор модели запроса добавления персонажа c анкетой пользователя
    /// </summary>
    /// <param name="user">Анкета пользователя</param>
    /// <param name="personalName">Личное имя</param>
    /// <param name="prefixNameId">Префикс имени</param>
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
    public AddHeroRequest(AddUserRequest? user, string? personalName, long? prefixNameId, string? familyName, string? birthDay,
        long? birthMonthId, string? birthCycle, long? nationId, bool? gender, string? height, string? weight, long? hairsColorId,
        long? eyesColorId, long? typeBodyId, long? typeFaceId, long? currentLocationId)
    {
        //Обрабатываем входные данные
        if (!int.TryParse(birthDay, out int dayBirth)) throw new InnerException(Errors.IncorrectDay);
        if (!int.TryParse(birthCycle, out int cycleBirth)) throw new InnerException(Errors.IncorrectCycle);
        if (!int.TryParse(height, out int heightValue)) throw new InnerException(Errors.IncorrectHeight);
        if (!int.TryParse(weight, out int weightValue)) throw new InnerException(Errors.IncorrectWeight);

        //Проверяем входные данные
        if (string.IsNullOrWhiteSpace(personalName)) throw new InnerException(Errors.EmptyPersonalName);
        if (string.IsNullOrWhiteSpace(familyName)) throw new InnerException(Errors.EmptyFamilyName);
        if (dayBirth == 0 || birthMonthId == null || cycleBirth == 0) throw new InnerException(Errors.EmptyBirthDate);
        if (dayBirth > 30 || BirthDay < 1) throw new InnerException(Errors.IncorrectDay);
        if (heightValue == 0) throw new InnerException(Errors.EmptyHeight);
        if (weightValue == 0) throw new InnerException(Errors.EmptyWeight);

        //Заполняем модель
        User = user ?? throw new InnerException(Errors.EmptyUser);
        PersonalName = personalName;
        PrefixNameId = prefixNameId ?? throw new InnerException(Errors.EmptyPrefixName);
        FamilyName = familyName;
        BirthDay = dayBirth;
        BirthMonthId = birthMonthId;
        BirthCycle = cycleBirth;
        NationId = nationId ?? throw new InnerException(Errors.EmptyNation);
        Gender = gender ?? throw new InnerException(Errors.EmptyGender);
        Height = heightValue;
        Weight = weightValue;
        HairsColorId = hairsColorId ?? throw new InnerException(Errors.EmptyHairsColor);
        EyesColorId = eyesColorId ?? throw new InnerException(Errors.EmptyEyesColor);
        TypeBodyId = typeBodyId ?? throw new InnerException(Errors.EmptyTypeBody);
        TypeFaceId = typeFaceId ?? throw new InnerException(Errors.EmptyTypeFace);
        CurrentLocationId = currentLocationId ?? throw new InnerException(Errors.EmptyArea);
    }

    /// <summary>
    /// Метод записи биографии
    /// </summary>
    /// <param name="biographies">Биографии</param>
    public void SetBiographies(List<AddBiographyHeroRequest> biographies)
    {
        Biographies = biographies;
    }
}