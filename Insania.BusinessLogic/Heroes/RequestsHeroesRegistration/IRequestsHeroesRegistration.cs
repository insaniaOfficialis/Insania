using Insania.Models.Heroes.RequestsHeroesRegistration;
using Insania.Models.OutCategories.Exceptions;

namespace Insania.BusinessLogic.Heroes.RequestsHeroesRegistration;

/// <summary>
/// Интерфейс работы с заявками на регистрацию персонажей
/// </summary>
public interface IRequestsHeroesRegistration
{
    /// <summary>
    /// Метод получения заявки на регистрацию персонажа по id
    /// </summary>
    /// <param name="id">Первичный ключ заявки</param>
    /// <returns cref="GetRequestRegistrationHeroResponse">Ответ</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    Task<GetRequestRegistrationHeroResponse> GetById(long? id);
}