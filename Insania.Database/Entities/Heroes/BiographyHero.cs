using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;
using Insania.Database.Entities.Chronology;

namespace Insania.Database.Entities.Heroes;

/// <summary>
/// Модель сущности биографии персонажа
/// </summary>
[Table("re_biographies_heroes")]
[Comment("Биографии персонажей")]
public class BiographyHero : Reestr
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
    /// День начала
    /// </summary>
    [Column("day_begin")]
    [Comment("День начала")]
    public int DayBegin { get; private set; }

    /// <summary>
    /// Ссылка на месяц начала
    /// </summary>
    [Column("month_begin_id")]
    [Comment("Ссылка на месяц начала")]
    public long MonthBeginId { get; private set; }

    /// <summary>
    /// Навигационное свойство месяца начала
    /// </summary>
    public Month MonthBegin { get; private set; }

    /// <summary>
    /// Цикл начала
    /// </summary>
    [Column("cycle_begin")]
    [Comment("Цикл начала")]
    public int CycleBegin { get; private set; }

    /// <summary>
    /// День окончания
    /// </summary>
    [Column("day_end")]
    [Comment("День окончания")]
    public int? DayEnd { get; private set; }

    /// <summary>
    /// Ссылка на месяц окончания
    /// </summary>
    [Column("month_end_id")]
    [Comment("Ссылка на месяц окончания")]
    public long? MonthEndId { get; private set; }

    /// <summary>
    /// Навигационное свойство месяца окончания
    /// </summary>
    public Month? MonthEnd { get; private set; }

    /// <summary>
    /// Цикл окончания
    /// </summary>
    [Column("cycle_end")]
    [Comment("Цикл окончания")]
    public int? CycleEnd { get; private set; }

    /// <summary>
    /// Текст
    /// </summary>
    [Column("text")]
    [Comment("Текст")]
    public string Text { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности биографии персонажа
    /// </summary>
    public BiographyHero() : base()
    {
        Hero = new();
        MonthBegin = new();
        Text = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности биографии персонажа без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="hero">Ссылка на персонажа</param>
    /// <param name="dayBegin">День начала</param>
    /// <param name="monthBegin">Ссылка на месяц начала</param>
    /// <param name="cycleBegin">Цикл начала</param>
    /// <param name="dayEnd">День окончания</param>
    /// <param name="monthEnd">Ссылка на месяц окончания</param>
    /// <param name="cycleEnd">Цикл окончания</param>
    /// <param name="text">Текст</param>
    public BiographyHero(string user, bool isSystem, Hero hero, int dayBegin, Month monthBegin, int cycleBegin, 
        int? dayEnd, Month? monthEnd, int? cycleEnd, string text) : base(user, isSystem)
    {
        HeroId = hero.Id;
        Hero = hero;
        DayBegin = dayBegin;
        MonthBeginId = monthBegin.Id;
        MonthBegin = monthBegin;
        CycleBegin = cycleBegin;
        DayEnd = dayEnd;
        MonthEndId = monthEnd?.Id;
        MonthEnd = monthEnd;
        CycleEnd = cycleEnd;
        Text = text;
    }

    /// <summary>
    /// Конструктор модели сущности биографии персонажа c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="hero">Ссылка на персонажа</param>
    /// <param name="dayBegin">День начала</param>
    /// <param name="monthBegin">Ссылка на месяц начала</param>
    /// <param name="cycleBegin">Цикл начала</param>
    /// <param name="dayEnd">День окончания</param>
    /// <param name="monthEnd">Ссылка на месяц окончания</param>
    /// <param name="cycleEnd">Цикл окончания</param>
    /// <param name="text">Текст</param>
    public BiographyHero(long id, string user, bool isSystem, Hero hero, int dayBegin, Month monthBegin,
        int cycleBegin, int? dayEnd, Month? monthEnd, int? cycleEnd, string text) : base(id, user, isSystem)
    {
        HeroId = hero.Id;
        Hero = hero;
        DayBegin = dayBegin;
        MonthBeginId = monthBegin.Id;
        MonthBegin = monthBegin;
        CycleBegin = cycleBegin;
        DayEnd = dayEnd;
        MonthEndId = monthEnd?.Id;
        MonthEnd = monthEnd;
        CycleEnd = cycleEnd;
        Text = text;
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
    /// Метод записи даты начала
    /// </summary>
    /// <param name="dayBegin">День начала</param>
    /// <param name="monthBegin">Ссылка на месяц начала</param>
    /// <param name="cycleBegin">Цикл начала</param>
    public void SetDateBegin(int dayBegin, Month monthBegin, int cycleBegin)
    {
        DayBegin = dayBegin;
        MonthBeginId = monthBegin.Id;
        MonthBegin = monthBegin;
        CycleBegin = cycleBegin;
    }

    /// <summary>
    /// Метод записи даты окончания
    /// </summary>
    /// <param name="dayEnd">День окончания</param>
    /// <param name="monthEnd">Ссылка на месяц окончания</param>
    /// <param name="cycleEnd">Цикл окончания</param>
    public void SetDateEnd(int dayEnd, Month monthEnd, int cycleEnd)
    {
        DayEnd = dayEnd;
        MonthEndId = monthEnd?.Id;
        MonthEnd = monthEnd;
        CycleEnd = cycleEnd;
    }

    /// <summary>
    /// Метод записи текста
    /// </summary>
    /// <param name="text">Текст</param>
    public void SetText(string text)
    {
        Text = text;
    }
}