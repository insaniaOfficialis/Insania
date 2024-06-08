using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;

namespace Insania.BusinessLogic.Heroes.StatusesRequestsHeroesRegistration;

/// <summary>
/// Интерфейс работы со статусами заявок на регистрацию персонажей
/// </summary>
public interface IStatusesRequestsHeroesRegistration
{
    /// <summary>
    /// Метод получения списка статусов заявок на регистрацию персонажей
    /// </summary>
    /// <returns cref="BaseResponseList">Базовая модель ответа для списка</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    Task<BaseResponseList> GetList();
}