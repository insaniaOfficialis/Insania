using Insania.Models.Heroes.BiographiesRequestsHeroesRegistration;
using Insania.Models.OutCategories.Exceptions;

namespace Insania.BusinessLogic.Heroes.BiographiesRequestsHeroesRegistration;

/// <summary>
/// Интерфейс работы с биографиями заявок на регистрацию персонажей
/// </summary>
public interface IBiographiesRequestsHeroesRegistration
{
    /// <summary>
    /// Метод получения биографии заявок на регистрацию персонажей по уникальному ключу
    /// </summary>
    /// <param name="biographyId">Биография</param>
    /// <param name="requestId">Заявка на регистрацию персонажа</param>
    /// <returns cref="GetBiographyRequestHeroRegistrationResponse">Модель ответа получения биографии завяки на регистрацию персонажа</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    Task<GetBiographyRequestHeroRegistrationResponse> GetByUnique(long? biographyId, long? requestId);
}