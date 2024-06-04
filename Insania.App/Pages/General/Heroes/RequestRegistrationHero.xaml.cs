using System.Collections.ObjectModel;

using Insania.BusinessLogic.Appearance.EyesColors;
using Insania.BusinessLogic.Appearance.HairsColors;
using Insania.BusinessLogic.Appearance.TypesBodies;
using Insania.BusinessLogic.Appearance.TypesFaces;
using Insania.BusinessLogic.Biology.Nations;
using Insania.BusinessLogic.Biology.Races;
using Insania.BusinessLogic.Chronology.Months;
using Insania.BusinessLogic.Files.Files;
using Insania.BusinessLogic.Heroes.Heroes;
using Insania.BusinessLogic.OutOfCategories.CheckConnection;
using Insania.BusinessLogic.Politics.Areas;
using Insania.BusinessLogic.Politics.Countries;
using Insania.BusinessLogic.Politics.Regions;
using Insania.BusinessLogic.Sociology.PrefixesNames;
using Insania.BusinessLogic.Users.Users;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Models.Users.Users;

namespace Insania.App.Pages.General.Heroes;

/// <summary>
/// Класс страницы заявки регистрации персонажа
/// </summary>
public partial class RequestRegistrationHero : ContentPage
{
    /// <summary>
    /// Первичный ключ персонажа
    /// </summary>
    private readonly long _id;

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
    /// Интерфейс работы с префиксами имён
    /// </summary>
    private readonly IPrefixesNames? _prefixNames;

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
    /// Интерфейс работы с типами телосложений
    /// </summary>
    private readonly ITypesBodies? _typesBodies;

    /// <summary>
    /// Интерфейс работы с типами лиц
    /// </summary>
    private readonly ITypesFaces? _typesFaces;

    /// <summary>
    /// Интерфейс работы с цветами волос
    /// </summary>
    private readonly IHairsColors? _hairsColors;

    /// <summary>
    /// Интерфейс работы с цветами глаз
    /// </summary>
    private readonly IEyesColors? _eyesColors;

    /// <summary>
    /// Интерфейс работы с пользователями
    /// </summary>
    private readonly IUsers? _users;

    /// <summary>
    /// Интерфейс работы с персонажами
    /// </summary>
    private readonly IHeroes? _heroes;

    /// <summary>
    /// Интерфейс работы с файлами
    /// </summary>
    private readonly IFiles? _files;


    /// <summary>
    /// Список рас
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Races { get; set; }

    /// <summary>
    /// Список наций
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Nations { get; set; }

    /// <summary>
    /// Список префиксов имён
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? PrefixesNames { get; set; }

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
    /// Список типов телосложений
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? TypesBodies { get; set; }

    /// <summary>
    /// Список типов лиц
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? TypesFaces { get; set; }

    /// <summary>
    /// Список цветов волос
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? HairsColors { get; set; }

    /// <summary>
    /// Список цветов глаз
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? EyesColors { get; set; }


    /// <summary>
    /// Конструктор класса страницы заявки регистрации персонажа
    /// </summary>
    /// <param name="id">Первичный ключ персонажа</param>
    public RequestRegistrationHero(long id)
	{
        //Инициализируем компоненты
        InitializeComponent();
        _id = id;

        //Получаем сервисы
        _checkConnection = App.Services?.GetService<ICheckConnection>();
        _races = App.Services?.GetService<IRaces>();
        _nations = App.Services?.GetService<INations>();
        _prefixNames = App.Services?.GetService<IPrefixesNames>();
        _months = App.Services?.GetService<IMonths>();
        _countries = App.Services?.GetService<ICountries>();
        _regions = App.Services?.GetService<IRegions>();
        _areas = App.Services?.GetService<IAreas>();
        _typesBodies = App.Services?.GetService<ITypesBodies>();
        _typesFaces = App.Services?.GetService<ITypesFaces>();
        _hairsColors = App.Services?.GetService<IHairsColors>();
        _eyesColors = App.Services?.GetService<IEyesColors>();
        _users = App.Services?.GetService<IUsers>();
        _heroes = App.Services?.GetService<IHeroes>();
        _files = App.Services?.GetService<IFiles>();
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
        RequestRegistrationHeroStackLayout.IsVisible = false;

        try
        {
            //Проверяем наличие сервисов
            if (_checkConnection == null) throw new InnerException(Errors.EmptyServiceCheckConnection);
            if (_races == null) throw new InnerException(Errors.EmptyServiceRaces);
            if (_months == null) throw new InnerException(Errors.EmptyServiceMonths);
            if (_countries == null) throw new InnerException(Errors.EmptyServiceСountries);
            if (_typesBodies == null) throw new InnerException(Errors.EmptyServiceTypesBodies);
            if (_typesFaces == null) throw new InnerException(Errors.EmptyServiceTypesFaces);
            if (_hairsColors == null) throw new InnerException(Errors.EmptyServiceHairsColors);
            if (_eyesColors == null) throw new InnerException(Errors.EmptyServiceEyesColors);
            if (_users == null) throw new InnerException(Errors.EmptyServiceUsers);
            if (_heroes == null) throw new InnerException(Errors.EmptyServiceHeroes);
            if (_files == null) throw new InnerException(Errors.EmptyServiceFiles);

            //Проверяем соединение
            if (!await _checkConnection.CheckNotAuthorize()) throw new InnerException(Errors.NoConnection);

            //Запускаем получение данных
            List<Task> tasks = [];
            Task<BaseResponseList> racesTask = _races.GetRacesList();
            tasks.Add(racesTask);
            Task<BaseResponseList> monthsTask = _months.GetMonthsList();
            tasks.Add(monthsTask);
            Task<BaseResponseList> countriesTask = _countries.GetCountriesList();
            tasks.Add(countriesTask);
            Task<BaseResponseList> typesBodiesTask = _typesBodies.GetTypesBodiesList();
            tasks.Add(typesBodiesTask);
            Task<BaseResponseList> typesFacesTask = _typesFaces.GetTypesFacesList();
            tasks.Add(typesFacesTask);
            Task<BaseResponseList> hairsColorsTask = _hairsColors.GetHairsColorsList();
            tasks.Add(hairsColorsTask);
            Task<BaseResponseList> eyesColorsTask = _eyesColors.GetEyesColorsList();
            tasks.Add(eyesColorsTask);
            await Task.WhenAll(tasks);

            //Получаем коллекци
            Races = new ObservableCollection<BaseResponseListItem>(racesTask.Result.Items!);
            Months = new ObservableCollection<BaseResponseListItem>(monthsTask.Result.Items!);
            Countries = new ObservableCollection<BaseResponseListItem>(countriesTask.Result.Items!);
            TypesBodies = new ObservableCollection<BaseResponseListItem>(typesBodiesTask.Result.Items!);
            TypesFaces = new ObservableCollection<BaseResponseListItem>(typesFacesTask.Result.Items!);
            HairsColors = new ObservableCollection<BaseResponseListItem>(hairsColorsTask.Result.Items!);
            EyesColors = new ObservableCollection<BaseResponseListItem>(eyesColorsTask.Result.Items!);

            //Привязываем данные
            RacePicker.ItemsSource = Races;
            MonthBirtPicker.ItemsSource = Months;
            CountryPicker.ItemsSource = Countries;
            TypeBodyPicker.ItemsSource = TypesBodies;
            TypeFacePicker.ItemsSource = TypesFaces;
            HairsColorPicker.ItemsSource = HairsColors;
            EyesColorPicker.ItemsSource = EyesColors;

            //Запускаем получение данных по персонажу
            tasks.Clear();
            
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
            RequestRegistrationHeroStackLayout.IsVisible = true;
        }
    }
}