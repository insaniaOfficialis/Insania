using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;
using Insania.Entities.Models.Geography;
using System.Diagnostics.Metrics;

namespace Insania.Entities.Models.Politics;

/// <summary>
/// Модель сущности области
/// </summary>
[Table("re_areas")]
[Comment("Области")]
public class Area : Reestr
{
    /// <summary>
    /// Наименование
    /// </summary>
    [Column("name")]
    [Comment("Наименование")]
    public string Name { get; private set; }

    /// <summary>
    /// Номер на карте
    /// </summary>
    [Column("number_on_map")]
    [Comment("Номер на карте")]
    public string NumberOnMap { get; private set; }

    /// <summary>
    /// Цвет на карте
    /// </summary>
    [Column("color_on_map")]
    [Comment("Цвет на карте")]
    public string ColorOnMap { get; private set; }

    /// <summary>
    /// Размер в пикселях
    /// </summary>
    [Column("pixel_size")]
    [Comment("Размер в пикселях")]
    public int PixelSize { get; private set; }

    /// <summary>
    /// Ссылка на страну
    /// </summary>
    [Column("country_id")]
    [Comment("Ссылка на страну")]
    public long CountryId { get; private set; }

    /// <summary>
    /// Навигационное свойство страны
    /// </summary>
    public Country Country { get; private set; }

    /// <summary>
    /// Ссылка на регион
    /// </summary>
    [Column("region_id")]
    [Comment("Ссылка на регион")]
    public long RegionId { get; private set; }

    /// <summary>
    /// Навигационное свойство региона
    /// </summary>
    public Region Region { get; private set; }

    /// <summary>
    /// Ссылка на географический объект
    /// </summary>
    [Column("geographical_object_id")]
    [Comment("Ссылка на географический объект")]
    public long GeographicalObjectId { get; private set; }

    /// <summary>
    /// Навигационное свойство географического объекта
    /// </summary>
    public GeographicalObject GeographicalObject { get; private set; }

    /// <summary>
    /// Ссылка на фракцию
    /// </summary>
    [Column("fraction_id")]
    [Comment("Ссылка на фракцию")]
    public long FractionId { get; private set; }

    /// <summary>
    /// Навигационное свойство фракции
    /// </summary>
    public Fraction Fraction { get; private set; }

    /// <summary>
    /// Ссылка на владение
    /// </summary>
    [Column("ownership_id")]
    [Comment("Ссылка на владение")]
    public long OwnershipId { get; private set; }

    /// <summary>
    /// Навигационное свойство владения
    /// </summary>
    public Ownership Ownership { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности области
    /// </summary>
    public Area() : base()
    {
        Name = string.Empty;
        NumberOnMap = string.Empty;
        ColorOnMap = string.Empty;
        Country = new();
        Region = new();
        GeographicalObject = new();
        Fraction = new();
        Ownership = new();
    }

    /// <summary>
    /// Конструктор модели сущности области без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="numberOnMap">Номер на карте/param>
    /// <param name="colorOnMap">Цвет на карте</param>
    /// <param name="pixelSize">Размер в пикселях</param>
    /// <param name="country">Ссылка на страну</param>
    /// <param name="region">Ссылка на регион</param>
    /// <param name="geographicalObject">Ссылка на географический объект</param>
    /// <param name="fraction">Ссылка на фракцию</param>
    /// <param name="ownership">Ссылка на владение</param>
    public Area(string user, bool isSystem, string name, string numberOnMap, string colorOnMap, int pixelSize,
        Country country, Region region, GeographicalObject geographicalObject, Fraction fraction, Ownership ownership)
        : base(user, isSystem)
    {
        Name = name;
        NumberOnMap = numberOnMap;
        ColorOnMap = colorOnMap;
        PixelSize = pixelSize;
        CountryId = country.Id;
        Country = country;
        RegionId = region.Id;
        Region = region;
        GeographicalObjectId = geographicalObject.Id;
        GeographicalObject = geographicalObject;
        FractionId = fraction.Id;
        Fraction = fraction;
        OwnershipId = ownership.Id;
        Ownership = ownership;
    }

    /// <summary>
    /// Конструктор модели сущности области с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="numberOnMap">Номер на карте/param>
    /// <param name="colorOnMap">Цвет на карте</param>
    /// <param name="pixelSize">Размер в пикселях</param>
    /// <param name="country">Ссылка на страну</param>
    /// <param name="region">Ссылка на регион</param>
    /// <param name="geographicalObject">Ссылка на географический объект</param>
    /// <param name="fraction">Ссылка на фракцию</param>
    /// <param name="ownership">Ссылка на владение</param>
    public Area(long id, string user, bool isSystem, string name, string numberOnMap, string colorOnMap, int pixelSize,
        Country country, Region region, GeographicalObject geographicalObject, Fraction fraction, Ownership ownership)
        : base(id, user, isSystem)
    {
        Name = name;
        NumberOnMap = numberOnMap;
        ColorOnMap = colorOnMap;
        PixelSize = pixelSize;
        CountryId = country.Id;
        Country = country;
        RegionId = region.Id;
        Region = region;
        GeographicalObjectId = geographicalObject.Id;
        GeographicalObject = geographicalObject;
        FractionId = fraction.Id;
        Fraction = fraction;
        OwnershipId = ownership.Id;
        Ownership = ownership;
    }

    /// <summary>
    /// Метод записи наименования
    /// </summary>
    /// <param name="name">Наименование</param>
    public void SetName(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Метод записи номера на карте
    /// </summary>
    /// <param name="numberOnMap">Номер на карте</param>
    public void SetNumberOnMap(string numberOnMap)
    {
        NumberOnMap = numberOnMap;
    }

    /// <summary>
    /// Метод записи цвета на карте
    /// </summary>
    /// <param name="colorOnMap">Цвет на карте</param>
    public void SetColorOnMap(string colorOnMap)
    {
        ColorOnMap = colorOnMap;
    }

    /// <summary>
    /// Метод записи размера в пикселях
    /// </summary>
    /// <param name="pixelSize">Размер в пикселях</param>
    public void SetPixelSize(int pixelSize)
    {
        PixelSize = pixelSize;
    }

    /// <summary>
    /// Метод записи страны
    /// </summary>
    /// <param name="country">Ссылка на страну</param>
    public void SetCountry(Country country)
    {
        CountryId = country.Id;
        Country = country;
    }

    /// <summary>
    /// Метод записи региона
    /// </summary>
    /// <param name="region">Ссылка на регион</param>
    public void SetRegion(Region region)
    {
        RegionId = region.Id;
        Region = region;
    }

    /// <summary>
    /// Метод записи географического объекта
    /// </summary>
    /// <param name="geographicalObject">Ссылка на географический объект</param>
    public void SetGeographicalObject(GeographicalObject geographicalObject)
    {
        GeographicalObjectId = geographicalObject.Id;
        GeographicalObject = geographicalObject;
    }

    /// <summary>
    /// Метод записи фракции
    /// </summary>
    /// <param name="fraction">Ссылка на фракцию</param>
    public void SetFraction(Fraction fraction)
    {
        FractionId = fraction.Id;
        Fraction = fraction;
    }

    /// <summary>
    /// Метод записи владения
    /// </summary>
    /// <param name="ownership">Ссылка на владение</param>
    public void SetOwnership(Ownership ownership)
    {
        OwnershipId = ownership.Id;
        Ownership = ownership;
    }
}