using Insania.Models.OutCategories.Base;

namespace Insania.BusinessLogic.Politics.Countries;

/// <summary>
/// Интерфейс работы со странами
/// </summary>
public interface ICountries
{
    /// <summary>
    /// Метод получения списка стран
    /// </summary>
    /// <returns></returns>
    Task<BaseResponseList> GetCountriesList();
}