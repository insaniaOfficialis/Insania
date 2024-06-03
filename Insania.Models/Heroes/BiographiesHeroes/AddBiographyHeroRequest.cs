using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.Models.Heroes.BiographiesHeroes;

/// <summary>
/// Модель запроса добавления биографии персонажа
/// </summary>
public class AddBiographyHeroRequest
{
    /// <summary>
    /// Ссылка на персонажа
    /// </summary>
    public long? HeroId { get; set; }

    /// <summary>
    /// День начала
    /// </summary>
    public int? DayBegin { get; set; }

    /// <summary>
    /// Ссылка на месяц начала
    /// </summary>
    public long? MonthBeginId { get; set; }

    /// <summary>
    /// Цикл начала
    /// </summary>
    public int? CycleBegin { get; set; }

    /// <summary>
    /// День окончания
    /// </summary>
    public int? DayEnd { get; set; }

    /// <summary>
    /// Ссылка на месяц окончания
    /// </summary>
    public long? MonthEndId { get; set; }

    /// <summary>
    /// Цикл окончания
    /// </summary>
    public int? CycleEnd { get; set; }

    /// <summary>
    /// Текст
    /// </summary>
    public string? Text { get; set; }

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
    public AddBiographyHeroRequest(string? dayBegin, long? monthBegin, string? cycleBegin, string? dayEnd, long? monthEnd,
        string? cycleEnd, string? text)
    {
        //Обрабатываем входные данные
        int endDay = 0, endCycle = 0;
        if (!int.TryParse(dayBegin, out int beginDay)) throw new InnerException(Errors.IncorrectDay);
        if (dayEnd != null && !int.TryParse(dayEnd, out endDay)) throw new InnerException(Errors.IncorrectDay);
        if (!int.TryParse(cycleBegin, out int beginCycle)) throw new InnerException(Errors.IncorrectCycle);
        if (cycleEnd != null && !int.TryParse(cycleEnd, out endCycle)) throw new InnerException(Errors.IncorrectCycle);

        //Проверяем входные данные
        if (beginDay == 0 || monthBegin == null || beginCycle == 0) throw new InnerException(Errors.EmptyBiographyhDate);
        if (string.IsNullOrWhiteSpace(text)) throw new InnerException(Errors.EmptyBiographyhText);
        if (beginDay > 30 || beginDay < 1) throw new InnerException(Errors.IncorrectDay);
        if (endDay != 0 && (endDay > 30 || endDay < 1)) throw new InnerException(Errors.IncorrectDay);

        //Заполняем модель
        DayBegin = beginDay;
        MonthBeginId = monthBegin;
        CycleBegin = beginCycle;
        DayEnd = endDay != 0 ? endDay : null;
        MonthEndId = monthEnd;
        CycleEnd = endCycle != 0 ? endCycle : null;
        Text = text;
    }
}