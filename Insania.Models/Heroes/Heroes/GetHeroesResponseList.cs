using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.Models.Heroes.Heroes;

/// <summary>
/// Модель ответа получения списка персонажей
/// </summary>
public class GetHeroesResponseList : BaseResponseList
{
    /// <summary>
    /// Список
    /// </summary>
    public new List<GetHeroesResponseListItem>? Items { get; set; }

    /// <summary>
    /// Простой конструктор модели ответа получения списка персонажей
    /// </summary>
    public GetHeroesResponseList() : base()
    {

    }

    /// <summary>
    /// Конструктор модели ответа получения списка персонажей
    /// </summary>
    /// <param name="success">Признак успешности</param>
    /// <param name="items">Элементы</param>
    /// <exception cref="Exception">Необработанное исключение</exception>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    public GetHeroesResponseList(bool success, List<GetHeroesResponseListItem>? items) : base(success)
    {
        //Проверяем входные данные
        if (items == null || items.Count == 0) throw new InnerException(Errors.EmptyHeroes);

        //Заполняем модель
        Items = items;
    }
}