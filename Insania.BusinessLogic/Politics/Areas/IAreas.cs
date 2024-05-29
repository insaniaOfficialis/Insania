using Insania.Models.OutCategories.Base;

namespace Insania.BusinessLogic.Politics.Areas;

/// <summary>
/// Интерфейс работы с областями
/// </summary>
public interface IAreas
{
    /// <summary>
    /// Метод получения списка областей
    /// </summary>
    /// <param name="regionId">Регион</param>
    /// <param name="ownershipId">Владение</param>
    /// <returns></returns>
    Task<BaseResponseList> GetAreasList(long? regionId, long? ownershipId);
}