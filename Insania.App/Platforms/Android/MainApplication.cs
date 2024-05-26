using Android.App;
using Android.Runtime;

namespace Insania.App;

/// <summary>
/// Основной класс приложения
/// </summary>
/// <param name="handle">Обработчик</param>
/// <param name="ownership">Владедель</param>
[Application]
public class MainApplication(IntPtr handle, JniHandleOwnership ownership) : MauiApplication(handle, ownership)
{
    /// <summary>
    /// Метод создания приложения
    /// </summary>
    /// <returns></returns>
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp()!;
}