using Insania.Models.Heroes.Heroes;
using Insania.Models.OutCategories.Base;

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
}