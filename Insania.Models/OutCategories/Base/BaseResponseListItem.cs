using System.Text.Json.Serialization;

using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.Models.OutCategories.Base;

/// <summary>
/// Модель ответа элемента списка
/// </summary>
public class BaseResponseListItem
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? Id { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    /// <summary>
    /// Простой конструктор модели ответа элемента списка
    /// </summary>
    public BaseResponseListItem()
    {

    }

    /// <summary>
    /// Конструктор модели элемента списка с id
    /// </summary>
    /// <param name="id"></param>
    public BaseResponseListItem(long? id)
    {
        Id = id;
    }

    /// <summary>
    /// Полный конструктор модели ответа элемента списка
    /// </summary>
    /// <param name="name"></param>
    /// <param name="id"></param>
    public BaseResponseListItem(long? id, string? name)
    {
        //Проверяем входные данные
        if ((id ?? 0) == 0) throw new InnerException(Errors.EmptyId);
        if (string.IsNullOrWhiteSpace(name)) throw new InnerException(Errors.EmptyName);

        //Заполняем модель
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Конструктор модели ответа элемента списка с наименованием
    /// </summary>
    /// <param name="name"></param>
    public BaseResponseListItem(string? name)
    {
        Name = name;
    }
}