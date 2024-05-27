using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Insania.App.Logic.OutCategories.CheckConnection;
using Insania.App.Logic.Users.Authentication;

namespace Insania.App;

/// <summary>
/// Класс запуска приложения
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Метод создания приложения
    /// </summary>
    /// <returns></returns>
    public static MauiApp? CreateMauiApp()
    {
        try
        {
            //Убираем подчёркивания у полей ввода
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
            {
#if ANDROID
                    handler.PlatformView.Background = null;
                    handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                    handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList
                        .ValueOf(Android.Graphics.Color.Transparent);
#elif WINDOWS
                    handler.PlatformView.BorderBrush = null;
                    handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            });

            //Убираем подчёркивания у полей ввода даты
            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
            {
#if ANDROID
                    handler.PlatformView.Background = null;
                    handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                    handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList
                        .ValueOf(Android.Graphics.Color.Transparent);
#elif WINDOWS
                    handler.PlatformView.BorderBrush = null;
                    handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            });

            //Формируем приложение
            var builder = MauiApp.CreateBuilder();

            //Добавляем шрифты
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Ofont_ru_Optimus.ttf", "Ofont_ru_Optimus");
                    fonts.AddFont("Georgia.ttf", "Georgia");
                });

            //Устанавливаем конфигурацию
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("Insania.App.appsettings.json");
            var config = new ConfigurationBuilder().AddJsonStream(stream!).Build();
            builder.Configuration.AddConfiguration(config);

#if DEBUG
            //Добавляем логгирование для дебага
            builder.Logging.AddDebug();
#endif
            //Добавляем сервисы
            builder.Services.AddScoped<ICheckConnection, CheckConnection>(); //проверка соединения
            builder.Services.AddScoped<IAuthentication, Authentication>(); //аутентифкация

            //Возвращаем построенное приложение
            return builder.Build();
        }
        catch (Exception ex)
        {
            //Выводим ошибку
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}