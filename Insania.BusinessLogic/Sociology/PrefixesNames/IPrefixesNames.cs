using Insania.Models.OutCategories.Base;

namespace Insania.BusinessLogic.Sociology.PrefixesNames;

/// <summary>
/// Интерфейс работы с префиксами имён
/// </summary>
public interface IPrefixesNames
{
    /// <summary>
    /// Метод получения списка префиксов имён
    /// </summary>
    /// <param name="nationId">Нация</param>
    /// <returns></returns>
    Task<BaseResponseList> GetList(long? nationId);
}