using Insania.App.Pages.General.Heroes;
using Insania.BusinessLogic.OutOfCategories.CheckConnection;
using Insania.BusinessLogic.Users.Users;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Models.Users.Users;

namespace Insania.App.Pages.General.Users;

/// <summary>
/// Класс страницы регистрации пользователя
/// </summary>
public partial class RegistrationUser : ContentPage
{
    /// <summary>
    /// Интерфейс проверки соединения
    /// </summary>
    private readonly ICheckConnection? _checkConnection;

    /// <summary>
    /// Интерфейс работы с пользователями
    /// </summary>
    private readonly IUsers? _users;

    /// <summary>
    /// Конструктор класса страницы регистрации пользователя
    /// </summary>
	public RegistrationUser()
    {
        //Инициализируем компоненты
        InitializeComponent();

        //Получаем сервисы
        _checkConnection = App.Services?.GetService<ICheckConnection>();
        _users = App.Services?.GetService<IUsers>();

        //Устанавливаем значения по умолчанию
        BirthDateDatePicker.Date = DateTime.Now.AddYears(-16).AddDays(1);
    }

    /// <summary>
    /// Событие загрузки окна
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        //Запускаем колесо загрузки
        LoadActivityIndicator.IsRunning = true;
        RegistrationUserStackLayout.IsVisible = false;

        try
        {
            //Проверяем наличие сервиса проверки соединения
            if (_checkConnection == null) throw new InnerException(Errors.EmptyServiceCheckConnection);

            //Проверяем, что подключается к сервису
            if (!await _checkConnection.CheckNotAuthorize()) throw new InnerException(Errors.NoConnection);
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
            RegistrationUserStackLayout.IsVisible = true;
        }
    }

    /// <summary>
    /// Событие нажатия на кнопку возвращения на предыдущую страницу
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void Back_Clicked(object sender, EventArgs e)
    {
        ToBack(null, null);
    }

    /// <summary>
    /// Событие нажатия на кнопку далее
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void Next_Clicked(object sender, EventArgs e)
    {
        //Запускаем колесо загрузки
        LoadActivityIndicator.IsRunning = true;
        RegistrationUserStackLayout.IsVisible = false;

        try
        {
            //Убираем клавитауру
            LoginEntry.IsEnabled = false;
            LoginEntry.IsEnabled = true;
            PasswordEntry.IsEnabled = false;
            PasswordEntry.IsEnabled = true;
            LastNameEntry.IsEnabled = false;
            LastNameEntry.IsEnabled = true;
            FirstNameEntry.IsEnabled = false;
            FirstNameEntry.IsEnabled = true;
            PatronymicEntry.IsEnabled = false;
            PatronymicEntry.IsEnabled = true;
            BirthDateDatePicker.IsEnabled = false;
            BirthDateDatePicker.IsEnabled = true;
            PhoneNumberEntry.IsEnabled = false;
            PhoneNumberEntry.IsEnabled = true;
            EmailEntry.IsEnabled = false;
            EmailEntry.IsEnabled = true;
            LinkVKEntry.IsEnabled = false;
            LinkVKEntry.IsEnabled = true;

            //Обнуляем текст ошибки
            ErrorLabel.Text = null;

            //Создаём модель запроса пользователя
            AddUserRequest addUserRequest = new(LoginEntry.Text, PasswordEntry.Text, LastNameEntry.Text, FirstNameEntry.Text,
                PatronymicEntry.Text, GenderCheckBox.IsChecked, BirthDateDatePicker.Date, PhoneNumberEntry.Text, EmailEntry.Text,
                LinkVKEntry.Text);

            //Проверяем доступность логина
            if (_users == null) throw new InnerException(Errors.EmptyServiceUsers);
            if (!(await _users.CheckLogin(addUserRequest.Login)).Success) throw new InnerException(Errors.LoginAlreadyExists);

            //Переходим на страницу регистрации героя
            ToRegistrationHero(addUserRequest);
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
            RegistrationUserStackLayout.IsVisible = true;
        }
    }

    /// <summary>
    /// Событие изменения пола
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void Gender_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //Если значение выбрано, ставим пол - мужской
        if (GenderCheckBox.IsChecked) GenderLabel.Text = "Мужской";
        //Иначе - женский
        else GenderLabel.Text = "Женский";
    }

    /// <summary>
    /// Метод возврата на предыдущую страницу
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void ToBack(object? sender, EventArgs? e)
    {
        //Переходим на старую страницу
        await Navigation.PopAsync();
    }

    /// <summary>
    /// Метод перехода на страницу регистрации героя
    /// </summary>
    /// <param name="addUserRequest">Модель добавления пользователя</param>
    private async void ToRegistrationHero(AddUserRequest addUserRequest)
    {
        //Переходим на старую страницу
        await Navigation.PushAsync(new RegistrationHero(addUserRequest));
    }
}