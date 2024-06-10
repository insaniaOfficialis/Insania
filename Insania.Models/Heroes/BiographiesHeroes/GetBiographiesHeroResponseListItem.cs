using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.Models.Heroes.BiographiesHeroes;

/// <summary>
/// Модель ответа получения биографии персонажа
/// </summary>
public class GetBiographiesHeroResponseListItem : BaseResponseListItem
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
    /// Простой конструктор модели ответа получения биографии персонажа
    /// </summary>
    public GetBiographiesHeroResponseListItem()
    {

    }

    /// <summary>
    /// Конструктор модели ответа получения биографии персонажа
    /// </summary>
    /// <param name="id">Первичный ключ сущности</param>
    /// <param name="heroId">Персонаж</param>
    /// <param name="dayBegin">День начала</param>
    /// <param name="monthBeginId">Месяц начала</param>
    /// <param name="cycleBegin">Цикл начала</param>
    /// <param name="dayEnd">День окончания</param>
    /// <param name="monthEndId">Месяц окончания</param>
    /// <param name="cycleEnd">Цикл окончания</param>
    /// <param name="text">Текст биографии</param>
    /// <exception cref="Exception">Необработанное исключение</exception>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    public GetBiographiesHeroResponseListItem(long? id, long? heroId, int? dayBegin, long? monthBeginId, int? cycleBegin,
        int? dayEnd, long? monthEndId, int? cycleEnd, string? text)
    {
        //Проверяем входные данные
        if ((id ?? 0) == 0) throw new InnerException(Errors.EmptyId);
        if ((heroId ?? 0) == 0) throw new InnerException(Errors.EmptyHero);
        if ((dayBegin ?? 0) == 0 || (monthBeginId ?? 0) == 0) throw new InnerException(Errors.EmptyBiographyhDate);
        if (dayBegin < 1 || dayBegin > 30 || ((dayEnd ?? 0) != 0 && (dayEnd < 1 || dayEnd > 30))) throw new InnerException(Errors.IncorrectBirthDate);
        if (string.IsNullOrWhiteSpace(text)) throw new InnerException(Errors.EmptyBiographyhText);

        //Заполняем модель
        Id = id;
        HeroId = heroId;
        DayBegin = dayBegin;
        MonthBeginId = monthBeginId;
        CycleBegin = cycleBegin;
        DayEnd = dayEnd;
        MonthEndId = monthEndId;
        CycleEnd = cycleEnd;
        Text = text;
    }
}