using Insania.Models.Heroes.BiographiesHeroes;
using Insania.Models.OutCategories.Base;

namespace Insania.Models.Heroes.Heroes;

/// <summary>
/// Модель ответа получения персонажа
/// </summary>
public class GetHeroResponse : BaseResponseListItem
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
    public List<AddBiographyHeroRequest>? Biographies { get; set; }
}
