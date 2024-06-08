using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;

namespace Insania.BusinessLogic.Administrators.Administrators;

/// <summary>
/// Интерфейс работы с администраторами
/// </summary>
public interface IAdministrators
{
    /// <summary>
    /// Метод получения списка администраторов
    /// </summary>
    /// <returns cref="BaseResponseList">Базовая модель ответа для списка</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    Task<BaseResponseList> GetList();
}