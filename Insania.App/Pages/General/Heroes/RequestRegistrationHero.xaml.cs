using System.Collections.ObjectModel;

using Insania.App.Pages.General.Users;
using Insania.App.Resources.Models.Heroes.BiographiesHeroes;
using Insania.BusinessLogic.Administrators.Administrators;
using Insania.BusinessLogic.Appearance.EyesColors;
using Insania.BusinessLogic.Appearance.HairsColors;
using Insania.BusinessLogic.Appearance.TypesBodies;
using Insania.BusinessLogic.Appearance.TypesFaces;
using Insania.BusinessLogic.Biology.Nations;
using Insania.BusinessLogic.Biology.Races;
using Insania.BusinessLogic.Chronology.Months;
using Insania.BusinessLogic.Files.Files;
using Insania.BusinessLogic.Heroes.BiographiesHeroes;
using Insania.BusinessLogic.Heroes.BiographiesRequestsHeroesRegistration;
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
using Insania.Models.Heroes.BiographiesHeroes;
using Insania.Models.Heroes.BiographiesRequestsHeroesRegistration;
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
    /// Интерфейс работы с биографиями персонажей
    /// </summary>
    private readonly IBiographiesHeroes? _biographiesHeroes;

    /// <summary>
    /// Интерфейс работы с биографиями заявок на регистрацию персонажей
    /// </summary>
    private readonly IBiographiesRequestsHeroesRegistration? _biographiesRequestsHeroesRegistration;


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
    /// Список биографий персонажа
    /// </summary>
    private ObservableCollection<GetBiographiesHeroResponseListItem>? BiographiesHeroes { get; set; }

    /// <summary>
    /// Коллеция элемнтов биографий
    /// </summary>
    private List<BiographyElement> BiographyElements { get; set; }

    
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
        BiographyElements = [];

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
        _biographiesHeroes = App.Services?.GetService<IBiographiesHeroes>();
        _biographiesRequestsHeroesRegistration = App.Services?.GetService<IBiographiesRequestsHeroesRegistration>();
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
            if (_biographiesHeroes == null) throw new InnerException(Errors.EmptyServiceBiographiesHeroes);
            if (_biographiesRequestsHeroesRegistration == null) throw new InnerException(Errors.EmptyServiceBiographiesRequestsHeroesRegistration);

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
            Task<GetBiographiesHeroResponseList> biographiesTask = _biographiesHeroes.GetList(HeroResponse.Id);
            tasks.Add(biographiesTask);
            await Task.WhenAll(tasks);

            //Получаем данные
            Nations = new ObservableCollection<BaseResponseListItem>(nationsTask.Result.Items!);
            PrefixesNames = new ObservableCollection<BaseResponseListItem>(prefixNamesTask.Result.Items!);
            Regions = new ObservableCollection<BaseResponseListItem>(regionsTask.Result.Items!);
            FileResponse = fileTask.Result;
            BiographiesHeroes = new ObservableCollection<GetBiographiesHeroResponseListItem>(biographiesTask.Result.Items!);

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
            if (RequestRegistrationHeroModel.GeneralBlockDecision != null)
            {
                GeneralBlockDecisionCheckBox.IsChecked = RequestRegistrationHeroModel.GeneralBlockDecision ?? true;
                GeneralBlockDecisionLabel.Text = RequestRegistrationHeroModel.GeneralBlockDecision == null ? "Неизвестно" : RequestRegistrationHeroModel.GeneralBlockDecision == true ? "Приятно" : "Не принято";
                if (RequestRegistrationHeroModel.GeneralBlockDecision == false)
                {
                    CommentOnGeneralBlockStackLayout.IsVisible = true;
                    CommentOnGeneralBlockEntry.Text = RequestRegistrationHeroModel.CommentOnGeneralBlock;
                }
            }
            DayBirthEntry.Text = HeroResponse.BirthDay.ToString();
            MonthBirtPicker.SelectedItem = Months.First(x => x.Id == HeroResponse.BirthMonthId) ?? throw new InnerException(Errors.EmptyMonth);
            CycleBirthEntry.Text = HeroResponse.BirthCycle.ToString();
            if (RequestRegistrationHeroModel.BirthDateBlockDecision != null)
            {
                BirthDateBlockDecisionCheckBox.IsChecked = RequestRegistrationHeroModel.BirthDateBlockDecision ?? true;
                BirthDateBlockDecisionLabel.Text = RequestRegistrationHeroModel.BirthDateBlockDecision == null ? "Неизвестно" : RequestRegistrationHeroModel.GeneralBlockDecision == true ? "Приятно" : "Не принято";
                if (RequestRegistrationHeroModel.BirthDateBlockDecision == false)
                {
                    CommentOnBirthDateBlockStackLayout.IsVisible = true;
                    CommentOnBirthDateBlockEntry.Text = RequestRegistrationHeroModel.CommentOnBirthDateBlock;
                }
            }
            CountryPicker.SelectedItem = Countries.First(x => x.Id == HeroResponse.CurrentCountryId) ?? throw new InnerException(Errors.EmptyCountry);
            RegionPicker.SelectedItem = Regions.First(x => x.Id == HeroResponse.CurrentRegionId) ?? throw new InnerException(Errors.EmptyRegion);
            AreaPicker.SelectedItem = Areas.First(x => x.Id == HeroResponse.CurrentLocationId) ?? throw new InnerException(Errors.EmptyArea);
            if (RequestRegistrationHeroModel.LocationBlockDecision != null)
            {
                LocationBlockDecisionCheckBox.IsChecked = RequestRegistrationHeroModel.LocationBlockDecision ?? true;
                LocationBlockDecisionLabel.Text = RequestRegistrationHeroModel.LocationBlockDecision == null ? "Неизвестно" : RequestRegistrationHeroModel.GeneralBlockDecision == true ? "Приятно" : "Не принято";
                if (RequestRegistrationHeroModel.LocationBlockDecision == false)
                {
                    CommentOnLocationBlockStackLayout.IsVisible = true;
                    CommentOnLocationBlockEntry.Text = RequestRegistrationHeroModel.CommentOnLocationBlock;
                }
            }
            TypeBodyPicker.SelectedItem = TypesBodies.First(x => x.Id == HeroResponse.TypeBodyId) ?? throw new InnerException(Errors.EmptyTypeBody);
            TypeFacePicker.SelectedItem = TypesFaces.First(x => x.Id == HeroResponse.TypeFaceId) ?? throw new InnerException(Errors.EmptyTypeFace);
            if (HeroResponse.HairsColorId != null) HairsColorPicker.SelectedItem = HairsColors.First(x => x.Id == HeroResponse.HairsColorId) ?? throw new InnerException(Errors.EmptyHairsColor);
            EyesColorPicker.SelectedItem = EyesColors.First(x => x.Id == HeroResponse.EyesColorId) ?? throw new InnerException(Errors.EmptyEyesColor);
            HeightEntry.Text = HeroResponse.Height.ToString();
            WeightEntry.Text = HeroResponse.Weight.ToString();
            if (RequestRegistrationHeroModel.AppearanceBlockDecision != null)
            {
                AppearanceBlockDecisionCheckBox.IsChecked = RequestRegistrationHeroModel.AppearanceBlockDecision ?? true;
                AppearanceBlockDecisionLabel.Text = RequestRegistrationHeroModel.AppearanceBlockDecision == null ? "Неизвестно" : RequestRegistrationHeroModel.GeneralBlockDecision == true ? "Приятно" : "Не принято";
                if (RequestRegistrationHeroModel.AppearanceBlockDecision == false)
                {
                    CommentOnAppearanceBlockStackLayout.IsVisible = true;
                    CommentOnAppearanceBlockEntry.Text = RequestRegistrationHeroModel.CommentOnAppearanceBlock;
                }
            }
            FileResponse.Stream!.Position = 0;
            HeroImage.Source = ImageSource.FromStream(() => FileResponse.Stream);
            if (RequestRegistrationHeroModel.ImageBlockDecision != null)
            {
                ImageBlockDecisionCheckBox.IsChecked = RequestRegistrationHeroModel.ImageBlockDecision ?? true;
                ImageBlockDecisionLabel.Text = RequestRegistrationHeroModel.ImageBlockDecision == null ? "Неизвестно" : RequestRegistrationHeroModel.GeneralBlockDecision == true ? "Приятно" : "Не принято";
                if (RequestRegistrationHeroModel.ImageBlockDecision == false)
                {
                    CommentOnImageBlockStackLayout.IsVisible = true;
                    CommentOnImageBlockEntry.Text = RequestRegistrationHeroModel.CommentOnImageBlock;
                }
            }

            //Отрисовываем биографии
            foreach (var item in BiographiesHeroes)
            {
                var decision = await _biographiesRequestsHeroesRegistration.GetByUnique(item.Id, RequestRegistrationHeroModel.Id);
                AddBiography(item, decision);
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

    /// <summary>
    /// Событие нажатия на кнопку добавления биографии
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void AddBiorgaphy_Clicked(object sender, EventArgs e)
    {
        //Обнуляем текст ошибки
        ErrorLabel.Text = null;

        try
        {
            //Добавляем биографию
            AddBiography();
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
    }
   
    /// <summary>
    /// Событие изменения решения по блоку биографии
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void BiographyDecision_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {

        //Отмечаем нажатые
        foreach (var item in BiographyElements)
        {
            //Если значение выбрано, ставим решение - принято
            if (item.Decision!.IsChecked) item.DecisionText!.Text = "Принято";
            //Иначе - не принято и выводим комментарий
            else
            {
                item.Comment!.IsVisible = true;
                item.DecisionText!.Text = "Не принято";
            }
        }
    }

    /// <summary>
    /// Событие нажатия на кнопку возвращения на предыдущую страницу
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void Back_Clicked(object sender, EventArgs e)
    {
        ToBack();
    }

    /// <summary>
    /// Событие нажатия кнопки назад
    /// </summary>
    /// <returns></returns>
    protected override bool OnBackButtonPressed()
    {
        ToBack();
        return true;
    }


    /// <summary>
    /// Метод добавления компонентов биографии
    /// </summary>
    private void AddBiography()
    {
        //Проверяем, что заполнили дату окончания у предыдущего
        if (BiographyElements.Count > 0
            && (string.IsNullOrWhiteSpace(BiographyElements.Last().DayEnd?.Text)
                || BiographyElements.Last().MonthEnd?.SelectedIndex == null
                || string.IsNullOrWhiteSpace(BiographyElements.Last().CycleEnd?.Text)))
            throw new InnerException(Errors.NotExistsDateEndBiography);

        //Пробуем получить данные для даты начала новой биографии
        string? autoDayStart = null;
        int? autoMonthStart = null;
        string? autoCycleEnd = null;
        if (BiographyElements.Count > 0)
        {
            autoDayStart = BiographyElements.Last().DayEnd?.Text;
            autoMonthStart = BiographyElements.Last().MonthEnd?.SelectedIndex;
            autoCycleEnd = BiographyElements.Last().CycleEnd?.Text;
        }
        else
        {
            autoDayStart = DayBirthEntry.Text;
            autoMonthStart = MonthBirtPicker.SelectedIndex;
            autoCycleEnd = CycleBirthEntry.Text;
        }

        //Получаем стили
        Application.Current!.Resources.TryGetValue("TitleSecondary", out var labelStyle);
        Application.Current!.Resources.TryGetValue("EntryPrimary", out var entryStyle);
        Application.Current!.Resources.TryGetValue("PrimaryText", out var boxViewColor);
        Application.Current!.Resources.TryGetValue("PickerPrimary", out var pickerStyle);


        //Создаём стек дня начала и добавляем в коллекцию биографий
        StackLayout dayBegin = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(dayBegin);

        //Создаём компоненты дня начала и добавляем в коллекцию
        Label dayBiographyBeginLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "День начала*:",
            Style = (Style)labelStyle
        };
        dayBegin.Add(dayBiographyBeginLabel);
        Entry dayBiographyBeginEntry = new()
        {
            Margin = new Thickness(0, 25, 0, 0),
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            Text = autoDayStart,
            Placeholder = "Введите день начала",
            Keyboard = Keyboard.Numeric,
            Style = (Style)entryStyle
        };
        dayBegin.Add(dayBiographyBeginEntry);
        BoxView dayBiographyBeginBoxView = new()
        {
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            HeightRequest = 1,
            Color = (Color)boxViewColor
        };
        dayBegin.Add(dayBiographyBeginBoxView);


        //Создаём стек месяца начала и добавляем в коллекцию биографий
        StackLayout monthBegin = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(monthBegin);

        //Создаём компоненты месяца начала и добавляем в коллекцию
        Label monthBiographyBeginLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Месяц начала*:",
            Style = (Style)labelStyle
        };
        monthBegin.Add(monthBiographyBeginLabel);
        Picker monthBiographyBeginPicker = new()
        {
            Margin = new Thickness(0, 25, 0, 0),
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
            Title = "Выберите месяц начала",
#endif
            ItemsSource = Months,
            ItemDisplayBinding = new Binding("Name"),
            SelectedIndex = autoMonthStart ?? -1,
            Style = (Style)pickerStyle
        };
        monthBegin.Add(monthBiographyBeginPicker);
        BoxView monthBiographyBeginBoxView = new()
        {
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            HeightRequest = 1,
            Color = (Color)boxViewColor
        };
        monthBegin.Add(monthBiographyBeginBoxView);


        //Создаём стек цикла начала и добавляем в коллекцию биографий
        StackLayout cycleBegin = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(cycleBegin);

        //Создаём компоненты цикла начала и добавляем в коллекцию
        Label cycleBiographyBeginLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Цикл начала*:",
            Style = (Style)labelStyle
        };
        cycleBegin.Add(cycleBiographyBeginLabel);
        Entry cycleBiographyBeginEntry = new()
        {
            Margin = new Thickness(0, 25, 0, 0),
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            Text = autoCycleEnd,
            Placeholder = "Введите цикл начала",
            Keyboard = Keyboard.Numeric,
            Style = (Style)entryStyle
        };
        cycleBegin.Add(cycleBiographyBeginEntry);
        BoxView cycleBiographyBeginBoxView = new()
        {
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            HeightRequest = 1,
            Color = (Color)boxViewColor
        };
        cycleBegin.Add(cycleBiographyBeginBoxView);


        //Создаём стек дня окончания и добавляем в коллекцию биографий
        StackLayout dayEnd = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(dayEnd);

        //Создаём компоненты дня окончания и добавляем в коллекцию
        Label dayBiographyEndLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "День окончания:",
            Style = (Style)labelStyle
        };
        dayEnd.Add(dayBiographyEndLabel);
        Entry dayBiographyEndEntry = new()
        {
            Margin = new Thickness(0, 25, 0, 0),
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            Placeholder = "Введите день окончания",
            Keyboard = Keyboard.Numeric,
            Style = (Style)entryStyle
        };
        dayEnd.Add(dayBiographyEndEntry);
        BoxView dayBiographyEndBoxView = new()
        {
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            HeightRequest = 1,
            Color = (Color)boxViewColor
        };
        dayEnd.Add(dayBiographyEndBoxView);


        //Создаём стек месяца окончания и добавляем в коллекцию биографий
        StackLayout monthEnd = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(monthEnd);

        //Создаём компоненты месяца окончания и добавляем в коллекцию
        Label monthBiographyEndLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Месяц окончания:",
            Style = (Style)labelStyle
        };
        monthEnd.Add(monthBiographyEndLabel);
        Picker monthBiographyEndPicker = new()
        {
            Margin = new Thickness(0, 25, 0, 0),
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
            Title = "Выберите месяц окончания",
#endif
            ItemsSource = Months,
            ItemDisplayBinding = new Binding("Name"),
            Style = (Style)pickerStyle
        };
        monthEnd.Add(monthBiographyEndPicker);
        BoxView monthBiographyEndBoxView = new()
        {
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            HeightRequest = 1,
            Color = (Color)boxViewColor
        };
        monthEnd.Add(monthBiographyEndBoxView);


        //Создаём стек цикла окончания и добавляем в коллекцию биографий
        StackLayout cycleEnd = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(cycleEnd);

        //Создаём компоненты цикла окончания и добавляем в коллекцию
        Label cycleBiographyEndLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Цикл окончания:",
            Style = (Style)labelStyle
        };
        cycleEnd.Add(cycleBiographyEndLabel);
        Entry cycleBiographyEndEntry = new()
        {
            Margin = new Thickness(0, 25, 0, 0),
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            Placeholder = "Введите цикл окончания",
            Keyboard = Keyboard.Numeric,
            Style = (Style)entryStyle
        };
        cycleEnd.Add(cycleBiographyEndEntry);
        BoxView cycleBiographyEndBoxView = new()
        {
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            HeightRequest = 1,
            Color = (Color)boxViewColor
        };
        cycleEnd.Add(cycleBiographyEndBoxView);


        //Создаём стек текста и добавляем в коллекцию биографий
        StackLayout text = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(text);

        //Создаём текст и добавляем в коллекцию
        Label textBiographyLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Текст биографии*:",
            Style = (Style)labelStyle
        };
        text.Add(textBiographyLabel);
        ScrollView textBiographyScroolView = new()
        {
#if WINDOWS
            MaximumHeightRequest = 400,
#else
            MaximumHeightRequest = 280,
#endif
            Margin = new Thickness(0, 25, 0, 0)
        };
        text.Add(textBiographyScroolView);
        Editor textBiographyEditor = new()
        {
            Margin = new Thickness(0, 0, 0, 25),
            Placeholder = "Введите текст биографии",
            AutoSize = EditorAutoSizeOption.TextChanges,
            Style = (Style)entryStyle
        };
        textBiographyScroolView.Content = textBiographyEditor;
        BoxView textBiographyBoxView = new()
        {
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            HeightRequest = 1,
            Color = (Color)boxViewColor
        };
        text.Add(textBiographyBoxView);

        //Создаём новый экземляр класса элемента биографии и добавляем его в коллекцию
        /*BiographyElement biographyElement = new(dayBiographyBeginEntry, monthBiographyBeginPicker, cycleBiographyBeginEntry,
            dayBiographyEndEntry, monthBiographyEndPicker, cycleBiographyEndEntry, textBiographyEditor);
        BiographyElements.Add(biographyElement);*/
    }

    /// <summary>
    /// Метод добавления компонентов биографии со значением
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <param name="decision">Решение</param>
    /// <exception cref="Exception">Необработанное исключение</exception>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    private void AddBiography(GetBiographiesHeroResponseListItem? request, GetBiographyRequestHeroRegistrationResponse? decision)
    {
        //Проверяем, что передали значения
        if (request == null) throw new InnerException(Errors.EmptyRequest);
        if (decision == null) throw new InnerException(Errors.EmtryDecision);

        //Проверяем, что заполнили дату окончания у предыдущего
        if (BiographyElements.Count > 0
            && (string.IsNullOrWhiteSpace(BiographyElements.Last().DayEnd?.Text)
                || BiographyElements.Last().MonthEnd?.SelectedIndex == null
                || string.IsNullOrWhiteSpace(BiographyElements.Last().CycleEnd?.Text)))
            throw new InnerException(Errors.NotExistsDateEndBiography);

        //Пробуем получить данные для даты начала новой биографии
        string? autoDayStart = null;
        int? autoMonthStart = null;
        string? autoCycleEnd = null;
        if (BiographyElements.Count > 0)
        {
            autoDayStart = BiographyElements.Last().DayEnd?.Text;
            autoMonthStart = BiographyElements.Last().MonthEnd?.SelectedIndex;
            autoCycleEnd = BiographyElements.Last().CycleEnd?.Text;
        }
        else
        {
            autoDayStart = DayBirthEntry.Text;
            autoMonthStart = MonthBirtPicker.SelectedIndex;
            autoCycleEnd = CycleBirthEntry.Text;
        }

        //Получаем стили
        Application.Current!.Resources.TryGetValue("TitleSecondary", out var labelStyle);
        Application.Current!.Resources.TryGetValue("EntryPrimary", out var entryStyle);
        Application.Current!.Resources.TryGetValue("PrimaryText", out var boxViewColor);
        Application.Current!.Resources.TryGetValue("PickerPrimary", out var pickerStyle);
        Application.Current!.Resources.TryGetValue("CheckBoxPrimary", out var checkBoxStyle);


        //Создаём заголовок блока
        Label numberTitleLabel = new()
        {
            HorizontalOptions = LayoutOptions.Center,
            Text = (BiographyElements.Count + 1).ToString(),
            Style = (Style)labelStyle
        };
        BiographyStackLayout.Add(numberTitleLabel);


        //Создаём стек дня начала и добавляем в коллекцию биографий
        StackLayout dayBegin = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(dayBegin);

        //Создаём компоненты дня начала и добавляем в коллекцию
        Label dayBiographyBeginLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "День начала*:",
            Style = (Style)labelStyle
        };
        dayBegin.Add(dayBiographyBeginLabel);
        Entry dayBiographyBeginEntry = new()
        {
            Margin = new Thickness(0, 25, 0, 0),
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            Text = request.DayBegin.ToString() ?? autoDayStart,
            Placeholder = "Введите день начала",
            Keyboard = Keyboard.Numeric,
            Style = (Style)entryStyle
        };
        dayBegin.Add(dayBiographyBeginEntry);
        BoxView dayBiographyBeginBoxView = new()
        {
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            HeightRequest = 1,
            Color = (Color)boxViewColor
        };
        dayBegin.Add(dayBiographyBeginBoxView);


        //Создаём стек месяца начала и добавляем в коллекцию биографий
        StackLayout monthBegin = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(monthBegin);

        //Создаём компоненты месяца начала и добавляем в коллекцию
        Label monthBiographyBeginLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Месяц начала*:",
            Style = (Style)labelStyle
        };
        monthBegin.Add(monthBiographyBeginLabel);
        Picker monthBiographyBeginPicker = new()
        {
            Margin = new Thickness(0, 25, 0, 0),
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
            Title = "Выберите месяц начала",
#endif
            ItemsSource = Months,
            ItemDisplayBinding = new Binding("Name"),
            SelectedIndex = Months?.IndexOf(Months.First(x => x.Id == request.MonthBeginId)) ?? autoMonthStart ?? -1,
            Style = (Style)pickerStyle
        };
        monthBegin.Add(monthBiographyBeginPicker);
        BoxView monthBiographyBeginBoxView = new()
        {
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            HeightRequest = 1,
            Color = (Color)boxViewColor
        };
        monthBegin.Add(monthBiographyBeginBoxView);


        //Создаём стек цикла начала и добавляем в коллекцию биографий
        StackLayout cycleBegin = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(cycleBegin);

        //Создаём компоненты цикла начала и добавляем в коллекцию
        Label cycleBiographyBeginLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Цикл начала*:",
            Style = (Style)labelStyle
        };
        cycleBegin.Add(cycleBiographyBeginLabel);
        Entry cycleBiographyBeginEntry = new()
        {
            Margin = new Thickness(0, 25, 0, 0),
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            Text = request.CycleBegin.ToString() ?? autoCycleEnd,
            Placeholder = "Введите цикл начала",
            Keyboard = Keyboard.Numeric,
            Style = (Style)entryStyle
        };
        cycleBegin.Add(cycleBiographyBeginEntry);
        BoxView cycleBiographyBeginBoxView = new()
        {
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            HeightRequest = 1,
            Color = (Color)boxViewColor
        };
        cycleBegin.Add(cycleBiographyBeginBoxView);


        //Создаём стек дня окончания и добавляем в коллекцию биографий
        StackLayout dayEnd = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(dayEnd);

        //Создаём компоненты дня окончания и добавляем в коллекцию
        Label dayBiographyEndLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "День окончания:",
            Style = (Style)labelStyle
        };
        dayEnd.Add(dayBiographyEndLabel);
        Entry dayBiographyEndEntry = new()
        {
            Margin = new Thickness(0, 25, 0, 0),
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            Text = request.DayEnd.ToString(),
            Placeholder = "Введите день окончания",
            Keyboard = Keyboard.Numeric,
            Style = (Style)entryStyle
        };
        dayEnd.Add(dayBiographyEndEntry);
        BoxView dayBiographyEndBoxView = new()
        {
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            HeightRequest = 1,
            Color = (Color)boxViewColor
        };
        dayEnd.Add(dayBiographyEndBoxView);


        //Создаём стек месяца окончания и добавляем в коллекцию биографий
        StackLayout monthEnd = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(monthEnd);

        //Создаём компоненты месяца окончания и добавляем в коллекцию
        Label monthBiographyEndLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Месяц окончания:",
            Style = (Style)labelStyle
        };
        monthEnd.Add(monthBiographyEndLabel);
        Picker monthBiographyEndPicker = new()
        {
            Margin = new Thickness(0, 25, 0, 0),
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
            Title = "Выберите месяц окончания",
#endif
            ItemsSource = Months,
            ItemDisplayBinding = new Binding("Name"),
            SelectedItem = request.MonthEndId != null ? Months?.First(x => x.Id == request.MonthEndId) : null,
            Style = (Style)pickerStyle
        };
        monthEnd.Add(monthBiographyEndPicker);
        BoxView monthBiographyEndBoxView = new()
        {
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            HeightRequest = 1,
            Color = (Color)boxViewColor
        };
        monthEnd.Add(monthBiographyEndBoxView);


        //Создаём стек цикла окончания и добавляем в коллекцию биографий
        StackLayout cycleEnd = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(cycleEnd);

        //Создаём компоненты цикла окончания и добавляем в коллекцию
        Label cycleBiographyEndLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Цикл окончания:",
            Style = (Style)labelStyle
        };
        cycleEnd.Add(cycleBiographyEndLabel);
        Entry cycleBiographyEndEntry = new()
        {
            Margin = new Thickness(0, 25, 0, 0),
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            Text = request.CycleEnd.ToString(),
            Placeholder = "Введите цикл окончания",
            Keyboard = Keyboard.Numeric,
            Style = (Style)entryStyle
        };
        cycleEnd.Add(cycleBiographyEndEntry);
        BoxView cycleBiographyEndBoxView = new()
        {
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            HeightRequest = 1,
            Color = (Color)boxViewColor
        };
        cycleEnd.Add(cycleBiographyEndBoxView);


        //Создаём стек текста и добавляем в коллекцию биографий
        StackLayout text = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(text);

        //Создаём текст и добавляем в коллекцию
        Label textBiographyLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Текст биографии*:",
            Style = (Style)labelStyle
        };
        text.Add(textBiographyLabel);
        ScrollView textBiographyScroolView = new()
        {
#if WINDOWS
            MaximumHeightRequest = 400,
#else
            MaximumHeightRequest = 280,
#endif
            Margin = new Thickness(0, 25, 0, 0)
        };
        text.Add(textBiographyScroolView);
        Editor textBiographyEditor = new()
        {
            Margin = new Thickness(0, 0, 0, 25),
            Text = request.Text,
            Placeholder = "Введите текст биографии",
            AutoSize = EditorAutoSizeOption.TextChanges,
            Style = (Style)entryStyle
        };
        textBiographyScroolView.Content = textBiographyEditor;
        BoxView textBiographyBoxView = new()
        {
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            HeightRequest = 1,
            Color = (Color)boxViewColor
        };
        text.Add(textBiographyBoxView);

        //Создаём блок решения
        Label decisionTitleLabel = new()
        {
            HorizontalOptions = LayoutOptions.Center,
            Text = "РЕШЕНИЕ",
            Style = (Style)labelStyle
        };
        BiographyStackLayout.Add(decisionTitleLabel);
        StackLayout biographyBlockDecisionStackLayout = new()
        {
            IsEnabled = false,
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(biographyBlockDecisionStackLayout);
        Label decisionLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Решение:",
            Style = (Style)labelStyle
        };
        biographyBlockDecisionStackLayout.Add(decisionLabel);
        StackLayout decisionStackLayout = new()
        {
            Orientation = StackOrientation.Horizontal
        };
        biographyBlockDecisionStackLayout.Add(decisionStackLayout);
        CheckBox decisionCheckBox = new()
        {
            IsChecked = decision.Decision == true,
            VerticalOptions = LayoutOptions.Center,
            Style = (Style)checkBoxStyle
        };
        decisionCheckBox.CheckedChanged += BiographyDecision_CheckedChanged!;
        decisionStackLayout.Add(decisionCheckBox);
        Label decisionTextLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center,
            Text = decision.Decision == null ? "Неизвестно" : decision.Decision == true ? "Принято" : "Не принято",
            Style = (Style)labelStyle
        };
        decisionStackLayout.Add(decisionTextLabel);
        StackLayout commentOnBiographyBlockStackLayout = new()
        {
            IsEnabled = false,
            IsVisible = decision.Decision == false,
            Margin = new Thickness(0, 25, 0, 0)
        };
        biographyBlockDecisionStackLayout.Add(commentOnBiographyBlockStackLayout);
        Label commentLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Комментарий*:",
            Style = (Style)labelStyle
        };
        commentOnBiographyBlockStackLayout.Add(commentLabel);
        Entry commentEntry = new()
        {
            Margin = new Thickness(0, 25, 0, 0),
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            Text = decision.Comment,
            Placeholder = "Введите комментарий",
            Keyboard = Keyboard.Numeric,
            Style = (Style)entryStyle
        };
        commentOnBiographyBlockStackLayout.Add(commentEntry);
        BoxView commentBoxView = new()
        {
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            HeightRequest = 1,
            Color = (Color)boxViewColor
        };
        commentOnBiographyBlockStackLayout.Add(commentBoxView);

        //Создаём новый экземляр класса элемента биографии и добавляем его в коллекцию
        BiographyElement biographyElement = new(dayBiographyBeginEntry, monthBiographyBeginPicker, cycleBiographyBeginEntry,
            dayBiographyEndEntry, monthBiographyEndPicker, cycleBiographyEndEntry, textBiographyEditor, decisionCheckBox,
            decisionTextLabel, commentEntry);
        BiographyElements.Add(biographyElement);
    }

    /// <summary>
    /// Метод возврата на предыдущую страницу
    /// </summary>
    private async void ToBack()
    {
        //Переходим на страницу авторизации
        await Navigation.PushModalAsync(new NavigationPage(new Authentication())
        {
            BarBackgroundColor = Color.FromArgb("#FF272727"),
            BarTextColor = Colors.Transparent
        });
    }
}