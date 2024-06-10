using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.Models.Heroes.BiographiesHeroes;

/// <summary>
/// Модель ответа получения списка биографий
/// </summary>
public class GetBiographiesHeroResponseList : BaseResponse
{
    /// <summary>
    /// Список
    /// </summary>
    public List<GetBiographiesHeroResponseListItem>? Items { get; set; }

    /// <summary>
    /// Простой конструктор модели ответа получения списка биографий
    /// </summary>
    public GetBiographiesHeroResponseList() : base()
    {

    }

    /// <summary>
    /// Конструктор модели ответа получения списка биографий
    /// </summary>
    /// <param name="success">Признак успешности</param>
    /// <param name="items">Элементы</param>
    /// <exception cref="Exception">Необработанное исключение</exception>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    public GetBiographiesHeroResponseList(bool success, List<GetBiographiesHeroResponseListItem>? items) : base(success)
    {
        //Проверяем входные данные
        if (items == null || items.Count == 0) throw new InnerException(Errors.EmptyBiographies);

        //Заполняем модель
        Items = items;
    }
}