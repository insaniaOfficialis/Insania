using System.Collections.ObjectModel;

using Insania.BusinessLogic.Administrators.Administrators;
using Insania.BusinessLogic.Appearance.EyesColors;
using Insania.BusinessLogic.Appearance.HairsColors;
using Insania.BusinessLogic.Appearance.TypesBodies;
using Insania.BusinessLogic.Appearance.TypesFaces;
using Insania.BusinessLogic.Biology.Nations;
using Insania.BusinessLogic.Biology.Races;
using Insania.BusinessLogic.Chronology.Months;
using Insania.BusinessLogic.Files.Files;
using Insania.BusinessLogic.Heroes.Heroes;
using Insania.BusinessLogic.Heroes.RequestsHeroesRegistration;
using Insania.BusinessLogic.Heroes.StatusesRequestsHeroesRegistration;
using Insania.BusinessLogic.OutOfCategories.CheckConnection;
using Insania.BusinessLogic.Politics.Areas;
using Insania.BusinessLogic.Politics.Countries;
using Insania.BusinessLogic.Politics.Regions;
using Insania.BusinessLogic.Sociology.PrefixesNames;
using Insania.BusinessLogic.Users.Users;
using Insania.Models.Files.Files;
using Insania.Models.Heroes.Heroes;
using Insania.Models.Heroes.RequestsHeroesRegistration;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

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
    /// Интерфейс работы с заявками на регистрацию персонажей
    /// </summary>
    private readonly IRequestsHeroesRegistration? _requestsHeroesRegistration;

    /// <summary>
    /// Интерфейс работы со статусами заявок на регистрацию персонажей
    /// </summary>
    private readonly IStatusesRequestsHeroesRegistration? _statusesRequestsHeroesRegistration;

    /// <summary>
    /// Интерфейс работы с администраторами
    /// </summary>
    private readonly IAdministrators? _administrators;


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
    /// Заявка на регистрацию персонажа
    /// </summary>
    private GetRequestRegistrationHeroResponse? RequestRegistrationHeroModel { get; set; }

    /// <summary>
    /// Список статусов заявок на регистрацию персонажей
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Statuses { get; set; }

    /// <summary>
    /// Список администраторов
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Administrators { get; set; }

    /// <summary>
    /// Персонаж
    /// </summary>
    private GetHeroResponse? HeroResponse { get; set; }

    /// <summary>
    /// Файл
    /// </summary>
    private GetFileResponse? FileResponse { get; set; }

    /// <summary>
    /// Признак идущей инициализации
    /// </summary>
    private bool Initialize { get; set; }

    /// <summary>
    /// Конструктор класса страницы заявки регистрации персонажа
    /// </summary>
    /// <param name="id">Первичный ключ персонажа</param>
    public RequestRegistrationHero(long id)
	{
        //Инициализируем компоненты
        InitializeComponent();
        _id = id;
        Initialize = false;

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
        _requestsHeroesRegistration = App.Services?.GetService<IRequestsHeroesRegistration>();
        _statusesRequestsHeroesRegistration = App.Services?.GetService<IStatusesRequestsHeroesRegistration>();
        _administrators = App.Services?.GetService<IAdministrators>();
    }

    /// <summary>
    /// Событие загрузки окна
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        //Записываем токен
        await SecureStorage.Default.SetAsync("token", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZGl2aW5pdGFzIiwiZXhwIjoxNzE4NTI0NDE5LCJpc3MiOiJBcGkiLCJhdWQiOiJBcHAifQ.kBNppHKxGSqpx2u5psWsg4hIsBCMcFex09Er9Au6EPY");

        //Запускаем колесо загрузки
        LoadActivityIndicator.IsVisible = true;
        LoadActivityIndicator.IsRunning = true;
        RequestRegistrationHeroStackLayout.IsVisible = false;

        try
        {
            //Устанавливаем, что идёт инициализация
            Initialize = true;

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
            if (_requestsHeroesRegistration == null) throw new InnerException(Errors.EmptyServiceRequestsHeroesRegistration);
            if (_statusesRequestsHeroesRegistration == null) throw new InnerException(Errors.EmptyServiceStatusesRequestsHeroesRegistration);
            if (_administrators == null) throw new InnerException(Errors.EmptyServiceAdministrators);
            if (_prefixNames == null) throw new InnerException(Errors.EmptyServicePrefixesNames);
            if (_nations == null) throw new InnerException(Errors.EmptyServiceNations);
            if (_regions == null) throw new InnerException(Errors.EmptyServiceRegions);
            if (_areas == null) throw new InnerException(Errors.EmptyServiceAreas);

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
            Task<GetRequestRegistrationHeroResponse> requestRegistrationHeroTask = _requestsHeroesRegistration.GetById(_id);
            tasks.Add(requestRegistrationHeroTask);
            Task<BaseResponseList> statusesTask = _statusesRequestsHeroesRegistration.GetList();
            tasks.Add(statusesTask);
            Task<BaseResponseList> administratorsTask = _administrators.GetList();
            tasks.Add(administratorsTask);
            await Task.WhenAll(tasks);

            //Получаем данные
            Races = new ObservableCollection<BaseResponseListItem>(racesTask.Result.Items!);
            Months = new ObservableCollection<BaseResponseListItem>(monthsTask.Result.Items!);
            Countries = new ObservableCollection<BaseResponseListItem>(countriesTask.Result.Items!);
            TypesBodies = new ObservableCollection<BaseResponseListItem>(typesBodiesTask.Result.Items!);
            TypesFaces = new ObservableCollection<BaseResponseListItem>(typesFacesTask.Result.Items!);
            HairsColors = new ObservableCollection<BaseResponseListItem>(hairsColorsTask.Result.Items!);
            EyesColors = new ObservableCollection<BaseResponseListItem>(eyesColorsTask.Result.Items!);
            RequestRegistrationHeroModel = requestRegistrationHeroTask.Result;
            Statuses = new ObservableCollection<BaseResponseListItem>(statusesTask.Result.Items!);
            Administrators = new ObservableCollection<BaseResponseListItem>(administratorsTask.Result.Items!);

            //Получаем персонажа
            HeroResponse = await _heroes.GetById(RequestRegistrationHeroModel.HeroId);

            //Запускаем получение данных
            tasks.Clear();
            Task<BaseResponseList> nationsTask = _nations.GetNationsList(HeroResponse.RaceId);
            tasks.Add(nationsTask);
            Task<BaseResponseList> prefixNamesTask = _prefixNames.GetList(HeroResponse.NationId);
            tasks.Add(prefixNamesTask);
            Task<BaseResponseList> regionsTask = _regions.GetRegionsList(HeroResponse.CurrentCountryId);
            tasks.Add(regionsTask);
            Task<GetFileResponse> fileTask = _files.GetById(HeroResponse.FileId);
            tasks.Add(fileTask);
            await Task.WhenAll(tasks);

            //Получаем данные
            Nations = new ObservableCollection<BaseResponseListItem>(nationsTask.Result.Items!);
            PrefixesNames = new ObservableCollection<BaseResponseListItem>(prefixNamesTask.Result.Items!);
            Regions = new ObservableCollection<BaseResponseListItem>(regionsTask.Result.Items!);
            FileResponse = fileTask.Result;

            //Запускаем получение данных
            tasks.Clear();
            Task<BaseResponseList> areasTask = _areas.GetAreasList(HeroResponse.CurrentRegionId, null);
            tasks.Add(areasTask);
            await Task.WhenAll(tasks);

            //Получаем данные
            Areas = new ObservableCollection<BaseResponseListItem>(areasTask.Result.Items!);

            //Привязываем данные
            RacePicker.ItemsSource = Races;
            MonthBirtPicker.ItemsSource = Months;
            CountryPicker.ItemsSource = Countries;
            TypeBodyPicker.ItemsSource = TypesBodies;
            TypeFacePicker.ItemsSource = TypesFaces;
            HairsColorPicker.ItemsSource = HairsColors;
            EyesColorPicker.ItemsSource = EyesColors;
            StatusPicker.ItemsSource = Statuses;
            AdministratorPicker.ItemsSource = Administrators;
            NationPicker.ItemsSource = Nations;
            PrefixNamePicker.ItemsSource = PrefixesNames;
            RegionPicker.ItemsSource = Regions;
            AreaPicker.ItemsSource = Areas;

            //Записываем данные
            StatusPicker.SelectedItem = Statuses.First(x => x.Id == RequestRegistrationHeroModel.StatusId) ?? throw new InnerException(Errors.EmptyStatusRequestsHeroesRegistration);
            if (RequestRegistrationHeroModel.AdministratorId != null) AdministratorPicker.SelectedItem = Administrators.First(x => x.Id == RequestRegistrationHeroModel.AdministratorId) ?? throw new InnerException(Errors.EmptyAdministrator);
            PersonalNameEntry.Text = HeroResponse.PersonalName;
            FamilyNameEntry.Text = HeroResponse.FamilyName;
            GenderCheckBox.IsChecked = HeroResponse.Gender ?? true;
            RacePicker.SelectedItem = Races.First(x => x.Id == HeroResponse.RaceId) ?? throw new InnerException(Errors.EmptyRace);
            NationPicker.SelectedItem = Nations.First(x => x.Id == HeroResponse.NationId) ?? throw new InnerException(Errors.EmptyNation);
            if (HeroResponse.PrefixNameId != null) PrefixNamePicker.SelectedItem = PrefixesNames.First(x => x.Id == HeroResponse.PrefixNameId) ?? throw new InnerException(Errors.EmptyNation);
            if (RequestRegistrationHeroModel.GeneralBlockDecision != null) GeneralBlockDecisionCheckBox.IsChecked = RequestRegistrationHeroModel.GeneralBlockDecision ?? true;
            if (RequestRegistrationHeroModel.GeneralBlockDecision == false)
            {
                CommentOnGeneralBlockStackLayout.IsVisible = true;
                CommentOnGeneralBlockEntry.Text = RequestRegistrationHeroModel.CommentOnGeneralBlock;
            }
            DayBirthEntry.Text = HeroResponse.BirthDay.ToString();
            MonthBirtPicker.SelectedItem = Months.First(x => x.Id == HeroResponse.BirthMonthId) ?? throw new InnerException(Errors.EmptyMonth);
            CycleBirthEntry.Text = HeroResponse.BirthCycle.ToString();
            if (RequestRegistrationHeroModel.BirthDateBlockDecision != null) BirthDateBlockDecisionCheckBox.IsChecked = RequestRegistrationHeroModel.BirthDateBlockDecision ?? true;
            if (RequestRegistrationHeroModel.BirthDateBlockDecision == false)
            {
                CommentOnBirthDateBlockStackLayout.IsVisible = true;
                CommentOnBirthDateBlockEntry.Text = RequestRegistrationHeroModel.CommentOnBirthDateBlock;
            }
            CountryPicker.SelectedItem = Countries.First(x => x.Id == HeroResponse.CurrentCountryId) ?? throw new InnerException(Errors.EmptyCountry);
            RegionPicker.SelectedItem = Regions.First(x => x.Id == HeroResponse.CurrentRegionId) ?? throw new InnerException(Errors.EmptyRegion);
            AreaPicker.SelectedItem = Areas.First(x => x.Id == HeroResponse.CurrentLocationId) ?? throw new InnerException(Errors.EmptyArea);
            if (RequestRegistrationHeroModel.LocationBlockDecision != null) LocationBlockDecisionCheckBox.IsChecked = RequestRegistrationHeroModel.LocationBlockDecision ?? true;
            if (RequestRegistrationHeroModel.LocationBlockDecision == false)
            {
                CommentOnLocationBlockStackLayout.IsVisible = true;
                CommentOnLocationBlockEntry.Text = RequestRegistrationHeroModel.CommentOnLocationBlock;
            }
            TypeBodyPicker.SelectedItem = TypesBodies.First(x => x.Id == HeroResponse.TypeBodyId) ?? throw new InnerException(Errors.EmptyTypeBody);
            TypeFacePicker.SelectedItem = TypesFaces.First(x => x.Id == HeroResponse.TypeFaceId) ?? throw new InnerException(Errors.EmptyTypeFace);
            if (HeroResponse.HairsColorId != null) HairsColorPicker.SelectedItem = HairsColors.First(x => x.Id == HeroResponse.HairsColorId) ?? throw new InnerException(Errors.EmptyHairsColor);
            EyesColorPicker.SelectedItem = EyesColors.First(x => x.Id == HeroResponse.EyesColorId) ?? throw new InnerException(Errors.EmptyEyesColor);
            HeightEntry.Text = HeroResponse.Height.ToString();
            WeightEntry.Text = HeroResponse.Weight.ToString();
            if (RequestRegistrationHeroModel.AppearanceBlockDecision != null) AppearanceBlockDecisionCheckBox.IsChecked = RequestRegistrationHeroModel.AppearanceBlockDecision ?? true;
            if (RequestRegistrationHeroModel.AppearanceBlockDecision == false)
            {
                CommentOnAppearanceBlockStackLayout.IsVisible = true;
                CommentOnAppearanceBlockEntry.Text = RequestRegistrationHeroModel.CommentOnAppearanceBlock;
            }
            FileResponse.Stream!.Position = 0;
            HeroImage.Source = ImageSource.FromStream(() => FileResponse.Stream);
            if (RequestRegistrationHeroModel.ImageBlockDecision != null) ImageBlockDecisionCheckBox.IsChecked = RequestRegistrationHeroModel.ImageBlockDecision ?? true;
            if (RequestRegistrationHeroModel.ImageBlockDecision == false)
            {
                CommentOnImageBlockStackLayout.IsVisible = true;
                CommentOnImageBlockEntry.Text = RequestRegistrationHeroModel.CommentOnImageBlock;
            }
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
            
            //Устанавливаем, что не идёт инициализация
            Initialize = false;
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
    /// Событие выбора расы
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void Race_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Если выбранный элемент пустой, выходим
        if (RacePicker.SelectedItem == null) return;

        //Если идёт инициализация, выходим
        if (Initialize) return;

        //Запускаем колесо загрузки
        NationLoadActivityIndicator.IsRunning = true;
        NationStackLayout.IsVisible = false;
        PrefixNameStackLayout.IsVisible = false;

        //Обнуляем текст ошибки
        ErrorLabel.Text = null;

        try
        {
            //Проверяем наличие сервиса работы с нациями
            if (_nations == null) throw new InnerException(Errors.EmptyServiceNations);

            //Получаем выбранную расу
            long? raceId = ((BaseResponseListItem?)RacePicker.SelectedItem)?.Id;

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
    /// Событие выбора нации
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void Nation_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Если выбранный элемент пустой, выходим
        if (NationPicker.SelectedItem == null) return;

        //Если идёт инициализация, выходим
        if (Initialize) return;

        //Запускаем колесо загрузки
        PrefixNameLoadActivityIndicator.IsRunning = true;
        PrefixNameStackLayout.IsVisible = false;

        //Обнуляем текст ошибки
        ErrorLabel.Text = null;

        try
        {
            //Проверяем наличие сервиса работы с префиксами имён
            if (_prefixNames == null) throw new InnerException(Errors.EmptyServicePrefixesNames);

            //Получаем выбранную нацию
            long? nationId = ((BaseResponseListItem?)NationPicker.SelectedItem)?.Id;

            //Получаем коллекцию
            PrefixesNames = new ObservableCollection<BaseResponseListItem>((await _prefixNames.GetList(nationId)).Items!);

            //Привязываем данные
            PrefixNamePicker.ItemsSource = PrefixesNames;

            //Делаем доступным выпадающий список наций
            PrefixNameStackLayout.IsVisible = true;
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
            PrefixNameLoadActivityIndicator.IsRunning = false;
        }
    }

    /// <summary>
    /// Событие изменения решения по блоку общее
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void GeneralBlockDecision_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //Если значение выбрано, ставим решение - принято
        if (GeneralBlockDecisionCheckBox.IsChecked) GeneralBlockDecisionLabel.Text = "Принято";
        //Иначе - не принято и выводим комментарий
        else
        {
            CommentOnGeneralBlockStackLayout.IsVisible = true;
            GeneralBlockDecisionLabel.Text = "Не принято";
        }
    }

    /// <summary>
    /// Событие изменения решения по блоку дата рождения
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void BirthDateBlockDecision_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //Если значение выбрано, ставим решение - принято
        if (BirthDateBlockDecisionCheckBox.IsChecked) BirthDateBlockDecisionLabel.Text = "Принято";
        //Иначе - не принято и выводим комментарий
        else
        {
            CommentOnBirthDateBlockStackLayout.IsVisible = true;
            BirthDateBlockDecisionLabel.Text = "Не принято";
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

        //Если идёт инициализация, выходим
        if (Initialize) return;

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

        //Если идёт инициализация, выходим
        if (Initialize) return;

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
    /// Событие изменения решения по блоку местоположение
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void LocationBlockDecision_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //Если значение выбрано, ставим решение - принято
        if (LocationBlockDecisionCheckBox.IsChecked) LocationBlockDecisionLabel.Text = "Принято";
        //Иначе - не принято и выводим комментарий
        else
        {
            CommentOnLocationBlockStackLayout.IsVisible = true;
            LocationBlockDecisionLabel.Text = "Не принято";
        }
    }

    /// <summary>
    /// Событие изменения решения по блоку внешность
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void AppearanceBlockDecision_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //Если значение выбрано, ставим решение - принято
        if (AppearanceBlockDecisionCheckBox.IsChecked) AppearanceBlockDecisionLabel.Text = "Принято";
        //Иначе - не принято и выводим комментарий
        else
        {
            CommentOnAppearanceBlockStackLayout.IsVisible = true;
            AppearanceBlockDecisionLabel.Text = "Не принято";
        }
    }

    /// <summary>
    /// Событие изменения решения по блоку изображение
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void ImageBlockDecision_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //Если значение выбрано, ставим решение - принято
        if (ImageBlockDecisionCheckBox.IsChecked) ImageBlockDecisionLabel.Text = "Принято";
        //Иначе - не принято и выводим комментарий
        else
        {
            CommentOnImageBlockStackLayout.IsVisible = true;
            ImageBlockDecisionLabel.Text = "Не принято";
        }
    }
}