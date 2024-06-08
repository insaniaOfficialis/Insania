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
using Insania.BusinessLogic.Heroes.RequestsHeroesRegistration;
using Insania.BusinessLogic.Heroes.StatusesRequestsHeroesRegistration;
using Insania.BusinessLogic.OutOfCategories.CheckConnection;
using Insania.BusinessLogic.Politics.Areas;
using Insania.BusinessLogic.Politics.Countries;
using Insania.BusinessLogic.Politics.Regions;
using Insania.BusinessLogic.Sociology.PrefixesNames;
using Insania.BusinessLogic.Users.Users;
using Insania.Models.Heroes.RequestsHeroesRegistration;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.App.Pages.General.Heroes;

/// <summary>
/// ����� �������� ������ ����������� ���������
/// </summary>
public partial class RequestRegistrationHero : ContentPage
{
    /// <summary>
    /// ��������� ���� ���������
    /// </summary>
    private readonly long _id;

    /// <summary>
    /// ��������� �������� ����������
    /// </summary>
    private readonly ICheckConnection? _checkConnection;

    /// <summary>
    /// ��������� ������ � ������
    /// </summary>
    private readonly IRaces? _races;

    /// <summary>
    /// ��������� ������ � �������
    /// </summary>
    private readonly INations? _nations;

    /// <summary>
    /// ��������� ������ � ���������� ���
    /// </summary>
    private readonly IPrefixesNames? _prefixNames;

    /// <summary>
    /// ��������� ������ � ��������
    /// </summary>
    private readonly IMonths? _months;

    /// <summary>
    /// ��������� ������ �� ��������
    /// </summary>
    private readonly ICountries? _countries;

    /// <summary>
    /// ��������� ������ � ���������
    /// </summary>
    private readonly IRegions? _regions;

    /// <summary>
    /// ��������� ������ � ���������
    /// </summary>
    private readonly IAreas? _areas;

    /// <summary>
    /// ��������� ������ � ������ ������������
    /// </summary>
    private readonly ITypesBodies? _typesBodies;

    /// <summary>
    /// ��������� ������ � ������ ���
    /// </summary>
    private readonly ITypesFaces? _typesFaces;

    /// <summary>
    /// ��������� ������ � ������� �����
    /// </summary>
    private readonly IHairsColors? _hairsColors;

    /// <summary>
    /// ��������� ������ � ������� ����
    /// </summary>
    private readonly IEyesColors? _eyesColors;

    /// <summary>
    /// ��������� ������ � ��������������
    /// </summary>
    private readonly IUsers? _users;

    /// <summary>
    /// ��������� ������ � �����������
    /// </summary>
    private readonly IHeroes? _heroes;

    /// <summary>
    /// ��������� ������ � �������
    /// </summary>
    private readonly IFiles? _files;

    /// <summary>
    /// ��������� ������ � �������� �� ����������� ����������
    /// </summary>
    private readonly IRequestsHeroesRegistration? _requestsHeroesRegistration;

    /// <summary>
    /// ��������� ������ �� ��������� ������ �� ����������� ����������
    /// </summary>
    private readonly IStatusesRequestsHeroesRegistration? _statusesRequestsHeroesRegistration;


    /// <summary>
    /// ������ ���
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Races { get; set; }

    /// <summary>
    /// ������ �����
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Nations { get; set; }

    /// <summary>
    /// ������ ��������� ���
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? PrefixesNames { get; set; }

    /// <summary>
    /// ������ �������
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Months { get; set; }

    /// <summary>
    /// ������ �����
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Countries { get; set; }

    /// <summary>
    /// ������ ��������
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Regions { get; set; }

    /// <summary>
    /// ������ ��������
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Areas { get; set; }

    /// <summary>
    /// ������ ����� ������������
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? TypesBodies { get; set; }

    /// <summary>
    /// ������ ����� ���
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? TypesFaces { get; set; }

    /// <summary>
    /// ������ ������ �����
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? HairsColors { get; set; }

    /// <summary>
    /// ������ ������ ����
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? EyesColors { get; set; }

    /// <summary>
    /// ������ �� ����������� ���������
    /// </summary>
    private GetRequestRegistrationHeroResponse? RequestRegistrationHeroModel { get; set; }

    /// <summary>
    /// ������ �������� ������ �� ����������� ����������
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Statuses { get; set; }


    /// <summary>
    /// ����������� ������ �������� ������ ����������� ���������
    /// </summary>
    /// <param name="id">��������� ���� ���������</param>
    public RequestRegistrationHero(long id)
	{
        //�������������� ����������
        InitializeComponent();
        _id = id;

        //�������� �������
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
    }

    /// <summary>
    /// ������� �������� ����
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        //��������� ������ ��������
        LoadActivityIndicator.IsVisible = true;
        LoadActivityIndicator.IsRunning = true;
        RequestRegistrationHeroStackLayout.IsVisible = false;

        try
        {
            //��������� ������� ��������
            if (_checkConnection == null) throw new InnerException(Errors.EmptyServiceCheckConnection);
            if (_races == null) throw new InnerException(Errors.EmptyServiceRaces);
            if (_months == null) throw new InnerException(Errors.EmptyServiceMonths);
            if (_countries == null) throw new InnerException(Errors.EmptyService�ountries);
            if (_typesBodies == null) throw new InnerException(Errors.EmptyServiceTypesBodies);
            if (_typesFaces == null) throw new InnerException(Errors.EmptyServiceTypesFaces);
            if (_hairsColors == null) throw new InnerException(Errors.EmptyServiceHairsColors);
            if (_eyesColors == null) throw new InnerException(Errors.EmptyServiceEyesColors);
            if (_users == null) throw new InnerException(Errors.EmptyServiceUsers);
            if (_heroes == null) throw new InnerException(Errors.EmptyServiceHeroes);
            if (_files == null) throw new InnerException(Errors.EmptyServiceFiles);
            if (_requestsHeroesRegistration == null) throw new InnerException(Errors.EmptyServiceRequestsHeroesRegistration);
            if (_statusesRequestsHeroesRegistration == null) throw new InnerException(Errors.EmptyServiceStatusesRequestsHeroesRegistration);

            //��������� ����������
            if (!await _checkConnection.CheckNotAuthorize()) throw new InnerException(Errors.NoConnection);

            //��������� ��������� ������
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
            await Task.WhenAll(tasks);

            //�������� ������
            Races = new ObservableCollection<BaseResponseListItem>(racesTask.Result.Items!);
            Months = new ObservableCollection<BaseResponseListItem>(monthsTask.Result.Items!);
            Countries = new ObservableCollection<BaseResponseListItem>(countriesTask.Result.Items!);
            TypesBodies = new ObservableCollection<BaseResponseListItem>(typesBodiesTask.Result.Items!);
            TypesFaces = new ObservableCollection<BaseResponseListItem>(typesFacesTask.Result.Items!);
            HairsColors = new ObservableCollection<BaseResponseListItem>(hairsColorsTask.Result.Items!);
            EyesColors = new ObservableCollection<BaseResponseListItem>(eyesColorsTask.Result.Items!);
            RequestRegistrationHeroModel = requestRegistrationHeroTask.Result;
            Statuses = new ObservableCollection<BaseResponseListItem>(statusesTask.Result.Items!);

            //����������� ������
            RacePicker.ItemsSource = Races;
            //RacePicker.SelectedItem = Races.First(x => x.Id == RequestRegistrationHeroModel.);
            MonthBirtPicker.ItemsSource = Months;
            CountryPicker.ItemsSource = Countries;
            TypeBodyPicker.ItemsSource = TypesBodies;
            TypeFacePicker.ItemsSource = TypesFaces;
            HairsColorPicker.ItemsSource = HairsColors;
            EyesColorPicker.ItemsSource = EyesColors;
            StatusPicker.ItemsSource = Statuses;
            StatusPicker.SelectedItem = Statuses.First(x => x.Id == RequestRegistrationHeroModel.Id) ?? throw new InnerException(Errors.EmptyStatusRequestsHeroesRegistration);

            //��������� ��������� ������ �� ���������
            tasks.Clear();
            
        }
        catch (InnerException ex)
        {
            //������������� ����� ������
            ErrorLabel.Text = ex.Message;
        }
        catch (Exception ex)
        {
            //������������� ����� ������
            ErrorLabel.Text = ex.Message;
        }
        finally
        {
            //������������� ������ ��������
            LoadActivityIndicator.IsVisible = false;
            LoadActivityIndicator.IsRunning = false;

            //���������� ��������� ���������
            RequestRegistrationHeroStackLayout.IsVisible = true;
        }
    }
}