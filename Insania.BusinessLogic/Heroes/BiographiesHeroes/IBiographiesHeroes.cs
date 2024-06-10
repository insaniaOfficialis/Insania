using Insania.Models.Heroes.BiographiesHeroes;
using Insania.Models.OutCategories.Exceptions;

namespace Insania.BusinessLogic.Heroes.BiographiesHeroes;

/// <summary>
/// Интерфейс работы с биографиями персонажей
/// </summary>
public interface IBiographiesHeroes
{
    /// <summary>
    /// Метод получения биографий персонажа
    /// </summary>
    /// <param name="heroId">Персонаж</param>
    /// <returns cref="GetBiographiesHeroResponseList">Модель ответа получения биографий персонажа</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    Task<GetBiographiesHeroResponseList> GetList(long? heroId);
}