using Insania.Models.OutCategories.Base;

namespace Insania.BusinessLogic.Appearance.TypesFaces;

/// <summary>
/// Интерфейс работы с типами лиц
/// </summary>
public interface ITypesFaces
{
    /// <summary>
    /// Метод получения списка типов лиц
    /// </summary>
    /// <returns></returns>
    Task<BaseResponseList> GetTypesFacesList();
}