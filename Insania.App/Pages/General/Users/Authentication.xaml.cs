using Insania.App.Pages.Desktop.OutCategories;
using Insania.App.Pages.General.Heroes;
using Insania.App.Pages.Mobile.OutCategories;
using Insania.BusinessLogic.Heroes.Heroes;
using Insania.BusinessLogic.Heroes.RequestsHeroesRegistration;
using Insania.BusinessLogic.OutOfCategories.CheckConnection;
using Insania.BusinessLogic.Users.Authentication;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Models.Users.Authentication;

namespace Insania.App.Pages.General.Users;

/// <summary>
/// Страница аутентификации
/// </summary>
public partial class Authentication : ContentPage
{
    /// <summary>
    /// Интерфейс проверки соединения
    /// </summary>
    private readonly ICheckConnection? _checkConnection;

    /// <summary>
    /// Интерфейс сервиса аутентификации
    /// </summary>
    private readonly IAuthentication? _authentication;

    /// <summary>
    /// Интерфейс работы с персонажами
    /// </summary>
    private readonly IHeroes? _heroes;

    /// <summary>
    /// Интерфейс работы с заявками на регистрацию персонажей
    /// </summary>
    private readonly IRequestsHeroesRegistration? _requestsHeroesRegistration;

    /// <summary>
    /// Конструктор страницы аутентификации
    /// </summary>
    public Authentication()
	{
        //Инициализируем компоненты
        InitializeComponent();

        //Получаем сервисы
        _checkConnection = App.Services?.GetService<ICheckConnection>();
        _authentication = App.Services?.GetService<IAuthentication>();
        _heroes = App.Services?.GetService<IHeroes>();
        _requestsHeroesRegistration = App.Services?.GetService<IRequestsHeroesRegistration>();
    }

    /// <summary>
    /// Событие загрузки окна
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        //Объявляем переменную ответа проверки соединения
        bool checkConnection = false;

        //Запускаем колесо загрузки
        LoadActivityIndicator.IsRunning = true;
        AuthenticationStackLayout.IsVisible = false;
        FeedbackButton.IsVisible = false;

        try
        {
            //Проверяем наличие сервиса проверки соединения
            if (_checkConnection == null) throw new InnerException(Errors.EmptyServiceCheckConnection);

            //Проверяем, что подключается к сервису
            if (!await _checkConnection.CheckNotAuthorize()) throw new InnerException(Errors.NoConnection);

            //Если есть токен и получилось авторизоваться по нему, переходим на главную
            if (!string.IsNullOrWhiteSpace(await SecureStorage.Default.GetAsync("token"))) checkConnection = await _checkConnection.CheckNotAuthorize();
        }
        catch(InnerException ex)
        {
            //Устанавливаем текст ошибки
            ErrorLabel.Text = ex.Message;
        }
        catch(Exception ex)
        {
            //Устанавливаем текст ошибки
            ErrorLabel.Text = ex.Message;
        }
        finally
        {
            //Останавливаем колесо загрузки
            LoadActivityIndicator.IsRunning = false;

            //Возвращаем видимость элементов
            AuthenticationStackLayout.IsVisible = true;
            FeedbackButton.IsVisible = true;
        }

        //Если проверка соединения прошла успешна, переходим на главную
        if (checkConnection) ToMain();
    }

    /// <summary>
    /// Событие нажатия на кнопку авторизации
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void Authorize_Clicked(object sender, EventArgs e)
    {
        //Объявляем переменную ответа аутентифкации
        AuthenticationResponse? result = new();

        //Запускаем колесо загрузки
        LoadActivityIndicator.IsRunning = true;
        AuthenticationStackLayout.IsVisible = false;
        FeedbackButton.IsVisible = false;

        try
        {
            //Убираем клавитауру
            LoginEntry.IsEnabled = false;
            LoginEntry.IsEnabled = true;
            PasswordEntry.IsEnabled = false;
            PasswordEntry.IsEnabled = true;

            //Обнуляем текст ошибки
            ErrorLabel.Text = null;

            //Вызываем метод авторизации
            if (_authentication == null) throw new InnerException(Errors.EmptyServiceAuthentication);
            result = await _authentication.Login(LoginEntry.Text, PasswordEntry.Text);

            //Получаем список текущих персонажей
            if (_heroes == null) throw new InnerException(Errors.EmptyServiceHeroes);
            var heroes = await _heroes.GetListByCurrent(null);

            //Если есть только один персонаж
            if (heroes != null && heroes.Items != null && heroes.Items.Count == 1)
            {
                //Проверяем наличие незавершённой заявки
                if (_requestsHeroesRegistration == null) throw new InnerException(Errors.EmptyServiceRequestsHeroesRegistration);
                var requestHeroRegistration = await _requestsHeroesRegistration.GetByHero(heroes.Items.First(x => x.IsCurrent == true).Id);

                //Если есть заявка без статуса "Принято"
                if (requestHeroRegistration != null && requestHeroRegistration.StatusId != 3)
                {
                    //Переходим на страницу заявки на регистрацию персонажа
                    ToRequestHeroRegistration(requestHeroRegistration.Id ?? 0);
                }
                else ToMain();

            }
            else ToMain();
        }
        catch (InnerException ex)
        {
            //Устанавливаем текст ошибки
            ErrorLabel.Text = ex.Message;
        }
        catch (Exception ex)
        {
            //Устанавливаем текст ошибки
            ErrorLabel.Text = ex.Message;
        }
        finally
        {
            //Останавливаем колесо загрузки
            LoadActivityIndicator.IsRunning = false;

            //Возвращаем видимость элементов
            AuthenticationStackLayout.IsVisible = true;
            FeedbackButton.IsVisible = true;
        }
    }

    /// <summary>
    /// Событие нажатия на кнопку восстановления пароля
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void RestorePassword_Clicked(object sender, EventArgs e)
    {
        //Переход на страницу восстановления пароля
        ToRestrorePassword(null, null);
    }

    /// <summary>
    /// Событие нажатия на кнопку регистрации
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void Registration_Clicked(object sender, EventArgs e)
    {
        //Переход на страницу регистрации
        ToRegistration(null, null);
    }

    /// <summary>
    /// Событие нажатия на кнопку обратной связи
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void Feedback_Clicked(object sender, EventArgs e)
    {
        await Task.Delay(500);
    }

    /// <summary>
    /// Событие нажатия кнопки назад
    /// </summary>
    /// <returns></returns>
    protected override bool OnBackButtonPressed()
    {
        return true;
    }

    /// <summary>
    /// Метод перехода на главную страницу
    /// </summary>
    private async void ToMain()
    {
        if (DeviceInfo.Idiom == DeviceIdiom.Desktop) await Navigation.PushModalAsync(new MainDesktop());
        else await Navigation.PushModalAsync(new MainMobile());
    }

    /// <summary>
    /// Метод перехода на страницу регистрации
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void ToRegistration(object? sender, EventArgs? e)
    {
        //Переходим на новую страницу
        await Navigation.PushAsync(new RegistrationUser());
    }

    /// <summary>
    /// Метод перехода на страницу восстановления пароля
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void ToRestrorePassword(object? sender, EventArgs? e)
    {
        //Переходим на новую страницу
        await Navigation.PushAsync(new RequestRegistrationHero(1));
    }

    /// <summary>
    /// Метод перехода на страницу заявки на регистрацию персонажа
    /// </summary>
    /// <param name="requestId">Заявка</param>
    public async void ToRequestHeroRegistration(long requestId)
    {
        //Переходим на новую страницу
        await Navigation.PushAsync(new RequestRegistrationHero(requestId));
    }
}