using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.Models.Files.Files;

/// <summary>
/// Модель запроса добавления файла
/// </summary>
public class AddFileRequest
{
    /// <summary>
    /// Первичный ключ сущности, на которую загружаются данные
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// Наименование файла
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Тип файла
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Поток файла
    /// </summary>
    public Stream? Stream { get; set; }

    /// <summary>
    /// Порядковый номер
    /// </summary>
    public long? OrdinalNumber { get; set; }

    /// <summary>
    /// Конструктор модели запроса добавления файла
    /// </summary>
    /// <param name="id">Первичный ключ сущности, на которую загружаются данные</param>
    /// <param name="name">Наименование файла</param>
    /// <param name="type">Тип файла</param>
    /// <param name="stream">Поток файла</param>
    /// <param name="ordinalNumber">Порядковый номер</param>
    public AddFileRequest(long? id, string? name, string? type, Stream? stream, long? ordinalNumber)
    {
        //Проверяем входные данные
        if (string.IsNullOrWhiteSpace(name)) throw new InnerException(Errors.EmptyNameFile);
        if (string.IsNullOrWhiteSpace(type)) throw new InnerException(Errors.EmptyTypeFile);

        //Заполняем модель
        Id = id ?? throw new InnerException(Errors.EmptyEntityFile);
        Name = name;
        Type = type;
        Stream = stream ?? throw new InnerException(Errors.EmptyStreamFile);
        OrdinalNumber = ordinalNumber;
    }
}