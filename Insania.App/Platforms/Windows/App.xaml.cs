namespace Insania.App.WinUI;

/// <summary>
/// Класс приложения windows
/// </summary>
public partial class App : MauiWinUIApplication
{
    /// <summary>
    /// Конструктор класса приложения windows
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Метод создания приложения
    /// </summary>
    /// <returns></returns>
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp()!;
}