using Insania.Models.OutCategories.Base;

namespace Insania.BusinessLogic.System.Parameters;

/// <summary>
/// Интерфейс работы с параметрами
/// </summary>
public interface IParameters
{
    /// <summary>
    /// Метод получения значения по алиасу
    /// </summary>
    /// <param name="alias">Псевдоним</param>
    /// <returns></returns>
    Task<BaseResponse> GetValueByAlias(string? alias);
}