namespace Insania.Models.OutCategories.Base;

/// <summary>
/// Базовая модель ответа для списка
/// </summary>
public class BaseResponseList : BaseResponse
{
    /// <summary>
    /// Список
    /// </summary>
    public List<BaseResponseListItem>? Items { get; set; }

    /// <summary>
    /// Простой конструктор базовой модели ответа для списка
    /// </summary>
    public BaseResponseList() : base()
    {

    }

    /// <summary>
    /// Конструктор базовой модели ответа для списка с ошибкой
    /// </summary>
    /// <param name="success">Признак успешности</param>
    /// <param name="error">Ошибка</param>
    public BaseResponseList(bool success, BaseError? error) : base(success, error)
    {

    }

    /// <summary>
    /// онструктор базовой модели ответа для списка с элементами
    /// </summary>
    /// <param name="success">Признак успешности</param>
    /// <param name="error">Ошибка</param>
    /// <param name="items">Элементы</param>
    public BaseResponseList(bool success, List<BaseResponseListItem>? items) : base(success)
    {
        Items = items;
    }
}