namespace Insania.App.Logic.OutCategories.CheckConnection;

/// <summary>
/// Интерфейс сервиса проверки соединения
/// </summary>
public interface ICheckConnection
{
    /// <summary>
    /// Метод проверки авторизованного пользователя
    /// </summary>
    /// <returns></returns>
    Task<bool> CheckAuthorize();

    /// <summary>
    /// Метод проверки неавторизованного пользователя
    /// </summary>
    /// <returns></returns>
    Task<bool> CheckNotAuthorize();
}