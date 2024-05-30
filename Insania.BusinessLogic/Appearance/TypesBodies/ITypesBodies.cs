using Insania.Models.OutCategories.Base;

namespace Insania.BusinessLogic.Appearance.TypesBodies;

/// <summary>
/// Интерфейс работы с типами телосложений
/// </summary>
public interface ITypesBodies
{
    /// <summary>
    /// Метод получения списка типов телосложений
    /// </summary>
    /// <returns></returns>
    Task<BaseResponseList> GetTypesBodiesList();
}