using Insania.App.Pages.General.Users;

namespace Insania.App;

/// <summary>
/// Класс основной страницы приложения
/// </summary>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Конструктор класса основой страницы приложения
    /// </summary>
    public MainPage()
    {
        //Инициализируем компоненты
        InitializeComponent();

        //Переходим к странице аутентификации
        ToAuthentication(null, null);
    }

    /// <summary>
    /// Метод перехода на страницу авторизации
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void ToAuthentication(object? sender, EventArgs? e)
    {
        //Переходим на новую страницу
        await Navigation.PushModalAsync(new NavigationPage(new Authentication())
        {
            BarBackgroundColor = Color.FromArgb("#FF272727"),
            BarTextColor = Colors.Transparent
        });
    }
}