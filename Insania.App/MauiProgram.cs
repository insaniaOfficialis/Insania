using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Insania.App.Logic.Appearance;
using Insania.App.Logic.Biology;
using Insania.App.Logic.Chronology;
using Insania.App.Logic.OutCategories;
using Insania.App.Logic.Polititcs;
using Insania.App.Logic.Users;
using Insania.BusinessLogic.Appearance.EyesColors;
using Insania.BusinessLogic.Appearance.HairsColors;
using Insania.BusinessLogic.Appearance.TypesBodies;
using Insania.BusinessLogic.Appearance.TypesFaces;
using Insania.BusinessLogic.Biology.Nations;
using Insania.BusinessLogic.Biology.Races;
using Insania.BusinessLogic.Chronology.Months;
using Insania.BusinessLogic.OutOfCategories.CheckConnection;
using Insania.BusinessLogic.Politics.Areas;
using Insania.BusinessLogic.Politics.Countries;
using Insania.BusinessLogic.Politics.Regions;
using Insania.BusinessLogic.Users.Authentication;
using Insania.BusinessLogic.Users.Users;

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

            //Убираем подчёркивания у выпадающих списков
            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
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
            
            //Убираем лишний эффект мигания для кнопок на android
            Microsoft.Maui.Handlers.ButtonHandler.Mapper.AppendToMapping("MyRippleCustomization", (handler, view) =>
            {
#if ANDROID
                if (handler.PlatformView.Background is Android.Graphics.Drawables.RippleDrawable ripple)
                {
                    ripple.SetColor(Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent));
                };
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
            builder.Services.AddScoped<ICheckConnection, CheckConnectionRequests>(); //проверка соединения
            builder.Services.AddScoped<IAuthentication, AuthenticationRequests>(); //аутентифкация
            builder.Services.AddScoped<IUsers, UsersRequests>(); //работа с пользователями
            builder.Services.AddScoped<IRaces, RacesRequests>(); //работа с расами
            builder.Services.AddScoped<INations, NationsRequests>(); //работа с нациями
            builder.Services.AddScoped<IMonths, MonthsRequests>(); //работа с месяцами
            builder.Services.AddScoped<ICountries, CountriesRequests>(); //работа со странами
            builder.Services.AddScoped<IRegions, RegionsRequests>(); //работа с регионами
            builder.Services.AddScoped<IAreas, AreasRequests>(); //работа с областями
            builder.Services.AddScoped<ITypesBodies, TypesBodiesRequests>(); //работа с типами телосложений
            builder.Services.AddScoped<ITypesFaces, TypesFacesRequests>(); //работа с типами лиц
            builder.Services.AddScoped<IHairsColors, HairsColorsRequests>(); //работа с цветами волос
            builder.Services.AddScoped<IEyesColors, EyesColorsRequests>(); //работа с цветами глаз

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