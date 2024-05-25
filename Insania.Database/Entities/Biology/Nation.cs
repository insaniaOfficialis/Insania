using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.Biology;

/// <summary>
/// Модель сущности нации
/// </summary>
[Table("dir_nations")]
[Comment("Нации")]
public class Nation : Guide
{
    /// <summary>
    /// Ссылка на расу
    /// </summary>
    [Column("race_id")]
    [Comment("Ссылка на расу")]
    public long RaceId { get; private set; }

    /// <summary>
    /// Навигационное свойство расы
    /// </summary>
    public Race Race { get; private set; }

    /// <summary>
    /// Язык для названий
    /// </summary>
    [Column("language_for_personal_names")]
    [Comment("Язык для имён")]
    public string LanguageForNames { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности нации
    /// </summary>
    public Nation() : base()
    {
        Race = new();
        LanguageForNames = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности нации без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    /// <param name="race">Ссылка на расу</param>
    /// <param name="languageForNames">Язык для имён</param>
    public Nation(string user, string name, Race race, string languageForNames) : base(user, name)
    {
        RaceId = race.Id;
        Race = race;
        LanguageForNames = languageForNames;
    }

    /// <summary>
    /// Конструктор модели сущности нации c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    /// <param name="race">Ссылка на расу</param>
    /// <param name="languageForNames">Язык для имён</param>
    public Nation(long id, string user, string name, Race race, string languageForNames) : base(id, user,
        name)
    {
        RaceId = race.Id;
        Race = race;
        LanguageForNames = languageForNames;
    }

    /// <summary>
    /// Метод записи расы
    /// </summary>
    /// <param name="race">Ссылка на расу</param>
    public void SetRace(Race race)
    {
        RaceId = race.Id;
        Race = race;
    }

    /// <summary>
    /// Метод записи языка для имён
    /// </summary>
    /// <param name="languageForNames">Язык для названий</param>
    public void SetLanguageForNames(string languageForNames)
    {
        LanguageForNames = languageForNames;
    }
}