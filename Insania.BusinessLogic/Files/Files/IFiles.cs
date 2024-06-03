using Insania.Models.Files.Files;
using Insania.Models.OutCategories.Base;

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
}