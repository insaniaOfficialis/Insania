using UIKit;

namespace Insania.App;

/// <summary>
/// Основной класс приложения
/// </summary>
public class Program
{
    /// <summary>
    /// Главный метод
    /// </summary>
    /// <param name="args">Параметры</param>
    static void Main(string[] args)
    {
        //Запуска приложение
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}