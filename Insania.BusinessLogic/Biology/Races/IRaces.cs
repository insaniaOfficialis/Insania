using Insania.Models.OutCategories.Base;

namespace Insania.BusinessLogic.Biology.Races;

/// <summary>
/// Интерфейс работы с расами
/// </summary>
public interface IRaces
{
    /// <summary>
    /// Метод получения списка рас
    /// </summary>
    /// <returns></returns>
    Task<BaseResponseList> GetRacesList();
}