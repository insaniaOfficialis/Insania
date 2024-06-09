using Insania.Models.Files.Files;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;

namespace Insania.BusinessLogic.Files.Files;

/// <summary>
/// Интерфейс работы с файлами
/// </summary>
public interface IFiles
{
    /// <summary>
    /// Метод добавления файла
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <returns></returns>
    Task<BaseResponse> Add(AddFileRequest? request);

    /// <summary>
    /// Метод получения файла по первичному ключу
    /// </summary>
    /// <param name="id">Первичный ключ</param>
    /// <returns cref="GetFileResponse">Модель ответа получения персонажа</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    Task<GetFileResponse> GetById(long? id);
}