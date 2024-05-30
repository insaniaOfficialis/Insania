using Insania.Models.OutCategories.Base;

namespace Insania.BusinessLogic.Appearance.HairsColors;

/// <summary>
/// Интерфейс работы с цветами волос
/// </summary>
public interface IHairsColors
{
    /// <summary>
    /// Метод получения списка цветов волос
    /// </summary>
    /// <returns></returns>
    Task<BaseResponseList> GetHairsColorsList();
}