namespace Insania.Models.Heroes.BiographiesHeroes;

/// <summary>
/// Модель запроса добавления биографии персонажа
/// </summary>
public class AddBiographyHeroRequest
{
    /// <summary>
    /// Ссылка на персонажа
    /// </summary>
    public long? HeroId { get; private set; }

    /// <summary>
    /// День начала
    /// </summary>
    public int? DayBegin { get; private set; }

    /// <summary>
    /// Ссылка на месяц начала
    /// </summary>
    public long? MonthBeginId { get; private set; }

    /// <summary>
    /// Цикл начала
    /// </summary>
    public int? CycleBegin { get; private set; }

    /// <summary>
    /// День окончания
    /// </summary>
    public int? DayEnd { get; private set; }

    /// <summary>
    /// Ссылка на месяц окончания
    /// </summary>
    public long? MonthEndId { get; private set; }

    /// <summary>
    /// Цикл окончания
    /// </summary>
    public int? CycleEnd { get; private set; }

    /// <summary>
    /// Текст
    /// </summary>
    public string? Text { get; private set; }

    /// <summary>
    /// Простой конструктор модели запроса добавления биографии персонажа
    /// </summary>
    public AddBiographyHeroRequest()
    {

    }

    /// <summary>
    /// Конструктор модели запроса добавления биографии персонажа без ссылки на персонажа
    /// </summary>
    /// <param name="dayBegin">День начала</param>
    /// <param name="monthBegin">Месяц начала</param>
    /// <param name="cycleBegin">Цикл начала</param>
    /// <param name="dayEnd">День окончания</param>
    /// <param name="monthEnd">Месяц окончания</param>
    /// <param name="cycleEnd">Цикл окончания</param>
    /// <param name="text">Текст</param>
    public AddBiographyHeroRequest(int? dayBegin, long? monthBegin, int? cycleBegin, int? dayEnd, long? monthEnd, int? cycleEnd,
        string? text)
    {
        DayBegin = dayBegin;
        MonthBeginId = monthBegin;
        CycleBegin = cycleBegin;
        DayEnd = dayEnd;
        MonthEndId = monthEnd;
        CycleEnd = cycleEnd;
        Text = text;
    }
}