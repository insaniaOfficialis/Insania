using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.Models.Files.Files;

/// <summary>
/// Модель ответа получения файла
/// </summary>
public class GetFileResponse : BaseResponse
{
    /// <summary>
    /// Поток файла
    /// </summary>
    public FileStream? FileStream { get; set; }
    
    /// <summary>
    /// Поток файла
    /// </summary>
    public Stream? Stream { get; set; }

    /// <summary>
    /// Тип контента
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// Простой конструктор модели ответа получения файла
    /// </summary>
    public GetFileResponse() : base()
    {

    }

    /// <summary>
    /// Конструктор модели ответа получения файла
    /// </summary>
    /// <param name="success">Признак успешности</param>
    /// <param name="id">Первичный ключ сущности</param>
    /// <param name="fileStream">Поток файла</param>
    /// <param name="contentType">Тип контента</param>
    /// <exception cref="Exception">Необработанное исключение</exception>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    public GetFileResponse(bool success, long id, FileStream? fileStream, string? contentType) : base(success, id)
    {
        // Проверяем входные данные
        if (fileStream == null) throw new InnerException(Errors.EmptyStreamFile);
        if (string.IsNullOrWhiteSpace(contentType)) throw new InnerException(Errors.EmptyTypeContent);

        //Заполняем модель
        FileStream = fileStream;
        ContentType = contentType;
    }

    /// <summary>
    /// Конструктор модели ответа получения файла
    /// </summary>
    /// <param name="success">Признак успешности</param>
    /// <param name="id">Первичный ключ сущности</param>
    /// <param name="stream">Поток файла</param>
    /// <param name="contentType">Тип контента</param>
    /// <exception cref="Exception">Необработанное исключение</exception>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    public GetFileResponse(bool success, long id, Stream? stream) : base(success, id)
    {
        // Проверяем входные данные
        if (stream == null) throw new InnerException(Errors.EmptyStreamFile);

        //Заполняем модель
        Stream = stream;
    }
}