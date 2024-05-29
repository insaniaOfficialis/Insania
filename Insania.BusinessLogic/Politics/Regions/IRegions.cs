using Insania.Models.OutCategories.Base;

namespace Insania.BusinessLogic.Politics.Regions;

/// <summary>
/// Интерфейс работы с регионом
/// </summary>
public interface IRegions
{
    /// <summary>
    /// Метод получения списка регионов
    /// </summary>
    /// <param name="countryId">Страна</param>
    /// <returns></returns>
    Task<BaseResponseList> GetRegionsList(long? countryId);
}