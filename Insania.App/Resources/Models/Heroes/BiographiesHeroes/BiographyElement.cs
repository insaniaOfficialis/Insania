namespace Insania.App.Resources.Models.Heroes.BiographiesHeroes;

/// <summary>
/// Класс элемента биографии
/// </summary>
/// <param name="dayBegin">Дата начала</param>
/// <param name="monthBegin">Месяц начала</param>
/// <param name="cycleBegin">Цикл начала</param>
/// <param name="dayEnd">День окончания</param>
/// <param name="monthEnd">Месяц окончания</param>
/// <param name="cycleEnd">Цикл окончания</param>
/// <param name="text">Текст</param>
/// <param name="decision">Решение</param>
/// <param name="decisionText">Текст решения</param>
/// <param name="comment">Комментарий к решению</param>
public class BiographyElement(Entry dayBegin, Picker monthBegin, Entry cycleBegin, Entry dayEnd, Picker monthEnd,
    Entry cycleEnd, Editor text, CheckBox? decision = null, Label? decisionText = null, Entry? comment = null)
{
    /// <summary>
    /// Поле ввода даты начала
    /// </summary>
    public Entry? DayBegin { get; set; } = dayBegin;

    /// <summary>
    /// Выпадающий список месяцев начала
    /// </summary>
    public Picker? MonthBegin { get; set; } = monthBegin;

    /// <summary>
    /// Поле ввода цикла начала
    /// </summary>
    public Entry? CycleBegin { get; set; } = cycleBegin;

    /// <summary>
    /// Поле ввода даты окончания
    /// </summary>
    public Entry? DayEnd { get; set; } = dayEnd;

    /// <summary>
    /// Выпадающий список месяцев окончания
    /// </summary>
    public Picker? MonthEnd { get; set; } = monthEnd;

    /// <summary>
    /// Поле ввода даты окончания
    /// </summary>
    public Entry? CycleEnd { get; set; } = cycleEnd;

    /// <summary>
    /// Поле ввода текст биографии
    /// </summary>
    public Editor? Text { get; set; } = text;

    /// <summary>
    /// Поле выбора решения
    /// </summary>
    public CheckBox? Decision { get; set; } = decision;

    /// <summary>
    /// Поле текста решения
    /// </summary>
    public Label? DecisionText { get; set; } = decisionText;

    /// <summary>
    /// Поле ввода комментария к решению
    /// </summary>
    public Entry? Comment { get; set; } = comment;
}