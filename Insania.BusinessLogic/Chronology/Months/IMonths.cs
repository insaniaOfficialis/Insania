using Insania.Models.OutCategories.Base;

namespace Insania.BusinessLogic.Chronology.Months;

/// <summary>
/// Интерфейс работы с месяцами
/// </summary>
public interface IMonths
{
    /// <summary>
    /// Метод получения списка месяцев
    /// </summary>
    /// <returns></returns>
    Task<BaseResponseList> GetMonthsList();
}