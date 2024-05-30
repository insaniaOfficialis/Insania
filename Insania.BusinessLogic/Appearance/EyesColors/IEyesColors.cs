using Insania.Models.OutCategories.Base;

namespace Insania.BusinessLogic.Appearance.EyesColors;

/// <summary>
/// Интерфейс работы с цветами глаз
/// </summary>
public interface IEyesColors
{
    /// <summary>
    /// Метод получения списка цветов глаз
    /// </summary>
    /// <returns></returns>
    Task<BaseResponseList> GetEyesColorsList();
}