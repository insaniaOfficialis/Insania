using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.Models.Heroes.Heroes;

/// <summary>
/// Модель элемента ответа получения списка персонажей
/// </summary>
public class GetHeroesResponseListItem : BaseResponseListItem
{
    /// <summary>
    /// Признак текущего
    /// </summary>
    public bool? IsCurrent { get; set; }

    /// <summary>
    /// Простой конструктор модели элемента ответа получения списка персонажей
    /// </summary>
    public GetHeroesResponseListItem()
    {

    }

    /// <summary>
    /// Конструктор модели элемента ответа получения списка персонажей
    /// </summary>
    /// <param name="id">Первичный ключ сущности</param>
    /// <param name="name">Наименование</param>
    /// <param name="isCurrent">Признак текущего</param>
    /// <exception cref="Exception">Необработанное исключение</exception>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    public GetHeroesResponseListItem(long? id, string? name, bool? isCurrent) : base(id, name)
    {
        //Заполняем модель
        IsCurrent = isCurrent ?? throw new InnerException(Errors.EmptyIsCurrent);
    }
}