using Insania.Models.Heroes.Heroes;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;

namespace Insania.BusinessLogic.Heroes.Heroes;

/// <summary>
/// Интерфейс работы с персонажами
/// </summary>
public interface IHeroes
{
    /// <summary>
    /// Метод регистрации персонажа
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <returns></returns>
    Task<BaseResponse> Registration(AddHeroRequest? request);

    /// <summary>
    /// Метод получения персонажа по первичному ключу
    /// </summary>
    /// <param name="id">Первичный ключ</param>
    /// <returns cref="GetHeroResponse">Модель ответа получения персонажа</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    Task<GetHeroResponse> GetById(long? id);
}