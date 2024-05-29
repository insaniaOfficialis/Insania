using Insania.Models.OutCategories.Base;

namespace Insania.BusinessLogic.Biology.Nations;

/// <summary>
/// Интерфейс работы с нациями
/// </summary>
public interface INations
{
    /// <summary>
    /// Метод получения списка наций
    /// </summary>
    /// <param name="raceId">Раса</param>
    /// <returns></returns>
    Task<BaseResponseList> GetNationsList(long? raceId);
}