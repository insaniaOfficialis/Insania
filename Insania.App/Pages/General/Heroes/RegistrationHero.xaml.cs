using System.Collections.ObjectModel;

using Insania.BusinessLogic.Biology.Nations;
using Insania.BusinessLogic.Biology.Races;
using Insania.BusinessLogic.Chronology.Months;
using Insania.BusinessLogic.OutOfCategories.CheckConnection;
using Insania.BusinessLogic.Politics.Areas;
using Insania.BusinessLogic.Politics.Countries;
using Insania.BusinessLogic.Politics.Regions;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Models.Users.Users;

namespace Insania.App.Pages.General.Heroes;

/// <summary>
/// Класс страницы регистрации персонажей
/// </summary>
public partial class RegistrationHero : ContentPage
{
    /// <summary>
    /// Интерфейс проверки соединения
    /// </summary>
    private readonly ICheckConnection? _checkConnection;

    /// <summary>
    /// Интерфейс работы с расами
    /// </summary>
    private readonly IRaces? _races;

    /// <summary>
    /// Интерфейс работы с нациями
    /// </summary>
    private readonly INations? _nations;

    /// <summary>
    /// Интерфейс работы с месяцами
    /// </summary>
    private readonly IMonths? _months;

    /// <summary>
    /// Интерфейс работы со странами
    /// </summary>
    private readonly ICountries? _countries;

    /// <summary>
    /// Интерфейс работы с регионами
    /// </summary>
    private readonly IRegions? _regions;

    /// <summary>
    /// Интерфейс работы с областями
    /// </summary>
    private readonly IAreas? _areas;

    /// <summary>
    /// Модель добавления пользователя
    /// </summary>
    private readonly AddUserRequest? _addUserRequest;

    /// <summary>
    /// Список рас
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Races { get; set; }

    /// <summary>
    /// Список наций
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Nations { get; set; }

    /// <summary>
    /// Список месяцев
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Months { get; set; }

    /// <summary>
    /// Список стран
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Countries { get; set; }

    /// <summary>
    /// Список регионов
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Regions { get; set; }

    /// <summary>
    /// Список областей
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Areas { get; set; }

    /// <summary>
    /// Конструктор класса страницы регистрации персонажа
    /// </summary>
    public RegistrationHero(AddUserRequest addUserRequest)
    {
        //Инициализируем компоненты
        InitializeComponent();

        //Получаем сервисы
        _checkConnection = App.Services?.GetService<ICheckConnection>();
        _races = App.Services?.GetService<IRaces>();
        _nations = App.Services?.GetService<INations>();
        _months = App.Services?.GetService<IMonths>();
        _countries = App.Services?.GetService<ICountries>();
        _regions = App.Services?.GetService<IRegions>();
        _areas = App.Services?.GetService<IAreas>();

        //Записываем входящие параметры
        _addUserRequest = addUserRequest;
    }

    /// <summary>
    /// Событие загрузки окна
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        //Запускаем колесо загрузки
        LoadActivityIndicator.IsVisible = true;
        LoadActivityIndicator.IsRunning = true;
        RegistrationHeroStackLayout.IsVisible = false;

        try
        {
            //Проверяем наличие сервиса проверки соединения
            if (_checkConnection == null) throw new InnerException(Errors.EmptyServiceCheckConnection);

            //Проверяем, что подключается к сервису
            if (!await _checkConnection.CheckNotAuthorize()) throw new InnerException(Errors.NoConnection);

            //Проверяем наличие сервиса работы с расами
            if (_races == null) throw new InnerException(Errors.EmptyServiceRaces);

            //Получаем коллекцию рас
            Races = new ObservableCollection<BaseResponseListItem>((await _races.GetRacesList()).Items!);

            //Привязываем данные
            RacePicker.ItemsSource = Races;

            //Проверяем наличие сервиса работы с месяцами
            if (_months == null) throw new InnerException(Errors.EmptyServiceMonths);

            //Получаем коллекцию месяцев
            Months = new ObservableCollection<BaseResponseListItem>((await _months.GetMonthsList()).Items!);

            //Привязываем данные
            BirtMonthPicker.ItemsSource = Months;

            //Проверяем наличие сервиса работы со странами
            if (_countries == null) throw new InnerException(Errors.EmptyServiceСountries);

            //Получаем коллекцию стран
            Countries = new ObservableCollection<BaseResponseListItem>((await _countries.GetCountriesList()).Items!);

            //Привязываем данные
            CountryPicker.ItemsSource = Countries;
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
            LoadActivityIndicator.IsVisible = false;
            LoadActivityIndicator.IsRunning = false;

            //Возвращаем видимость элементов
            RegistrationHeroStackLayout.IsVisible = true;
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
    /// Событие выбора расы
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void Race_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Если выбранный элемент пустой, выходим
        if (RacePicker.SelectedItem ==  null) return;

        //Запускаем колесо загрузки
        NationLoadActivityIndicator.IsRunning = true;
        NationStackLayout.IsVisible = false;

        //Обнуляем текст ошибки
        ErrorLabel.Text = null;

        try
        {
            //Проверяем наличие сервиса работы с нациями
            if (_nations == null) throw new InnerException(Errors.EmptyServiceNations);

            //Получаем выбранную расу
            long? raceId = ((BaseResponseListItem?) RacePicker.SelectedItem)?.Id;

            //Получаем коллекцию наций
            Nations = new ObservableCollection<BaseResponseListItem>((await _nations.GetNationsList(raceId)).Items!);

            //Привязываем данные
            NationPicker.ItemsSource = Nations;

            //Делаем доступным выпадающий список наций
            NationStackLayout.IsVisible = true;
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
            NationLoadActivityIndicator.IsRunning = false;
        }
    }

    /// <summary>
    /// Событие выбора страны
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void Country_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Если выбранный элемент пустой, выходим
        if (CountryPicker.SelectedItem == null) return;

        //Запускаем колесо загрузки
        RegionLoadActivityIndicator.IsRunning = true;
        RegionStackLayout.IsVisible = false;
        AreaStackLayout.IsVisible = false;

        //Обнуляем текст ошибки
        ErrorLabel.Text = null;

        try
        {
            //Проверяем наличие сервиса работы с регионами
            if (_regions == null) throw new InnerException(Errors.EmptyServiceRegions);

            //Получаем выбранную страну
            long? countryId = ((BaseResponseListItem?)CountryPicker.SelectedItem)?.Id;

            //Получаем коллекцию регионов
            Regions = new ObservableCollection<BaseResponseListItem>((await _regions.GetRegionsList(countryId)).Items!);

            //Привязываем данные
            RegionPicker.ItemsSource = Regions;

            //Делаем доступным выпадающий список регионов
            RegionStackLayout.IsVisible = true;
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
            RegionLoadActivityIndicator.IsRunning = false;
        }
    }

    /// <summary>
    /// Событие выбора региона
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void Region_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Если выбранный элемент пустой, выходим
        if (RegionPicker.SelectedItem == null) return;

        //Запускаем колесо загрузки
        AreaLoadActivityIndicator.IsRunning = true;
        AreaStackLayout.IsVisible = false;

        //Обнуляем текст ошибки
        ErrorLabel.Text = null;

        try
        {
            //Проверяем наличие сервиса работы с областями
            if (_areas == null) throw new InnerException(Errors.EmptyServiceAreas);

            //Получаем выбранный регион
            long? regionId = ((BaseResponseListItem?)RegionPicker.SelectedItem)?.Id;

            //Получаем коллекцию областей
            Areas = new ObservableCollection<BaseResponseListItem>((await _areas.GetAreasList(regionId, null)).Items!);

            //Привязываем данные
            AreaPicker.ItemsSource = Areas;

            //Делаем доступным выпадающий список областей
            AreaStackLayout.IsVisible = true;
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
            AreaLoadActivityIndicator.IsRunning = false;
        }
    }

    /// <summary>
    /// Событие нажатия на кнопку сохранения
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void Save_Clicked(object sender, EventArgs e)
    {

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
}