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
    /// ��������� ������ � ����������������
    /// </summary>
    private readonly IAdministrators? _administrators;


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
    /// ������ ���������������
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Administrators { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    private GetHeroResponse? HeroResponse { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    private GetFileResponse? FileResponse { get; set; }

    /// <summary>
    /// ������� ������ �������������
    /// </summary>
    private bool Initialize { get; set; }

    /// <summary>
    /// ����������� ������ �������� ������ ����������� ���������
    /// </summary>
    /// <param name="id">��������� ���� ���������</param>
    public RequestRegistrationHero(long id)
	{
        //�������������� ����������
        InitializeComponent();
        _id = id;
        Initialize = false;

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
        _administrators = App.Services?.GetService<IAdministrators>();
    }

    /// <summary>
    /// ������� �������� ����
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        //���������� �����
        await SecureStorage.Default.SetAsync("token", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZGl2aW5pdGFzIiwiZXhwIjoxNzE4NTI0NDE5LCJpc3MiOiJBcGkiLCJhdWQiOiJBcHAifQ.kBNppHKxGSqpx2u5psWsg4hIsBCMcFex09Er9Au6EPY");

        //��������� ������ ��������
        LoadActivityIndicator.IsVisible = true;
        LoadActivityIndicator.IsRunning = true;
        RequestRegistrationHeroStackLayout.IsVisible = false;

        try
        {
            //�������������, ��� ��� �������������
            Initialize = true;

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
            if (_administrators == null) throw new InnerException(Errors.EmptyServiceAdministrators);
            if (_prefixNames == null) throw new InnerException(Errors.EmptyServicePrefixesNames);
            if (_nations == null) throw new InnerException(Errors.EmptyServiceNations);
            if (_regions == null) throw new InnerException(Errors.EmptyServiceRegions);
            if (_areas == null) throw new InnerException(Errors.EmptyServiceAreas);

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
            Task<BaseResponseList> administratorsTask = _administrators.GetList();
            tasks.Add(administratorsTask);
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
            Administrators = new ObservableCollection<BaseResponseListItem>(administratorsTask.Result.Items!);

            //�������� ���������
            HeroResponse = await _heroes.GetById(RequestRegistrationHeroModel.HeroId);

            //��������� ��������� ������
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

            //�������� ������
            Nations = new ObservableCollection<BaseResponseListItem>(nationsTask.Result.Items!);
            PrefixesNames = new ObservableCollection<BaseResponseListItem>(prefixNamesTask.Result.Items!);
            Regions = new ObservableCollection<BaseResponseListItem>(regionsTask.Result.Items!);
            FileResponse = fileTask.Result;

            //��������� ��������� ������
            tasks.Clear();
            Task<BaseResponseList> areasTask = _areas.GetAreasList(HeroResponse.CurrentRegionId, null);
            tasks.Add(areasTask);
            await Task.WhenAll(tasks);

            //�������� ������
            Areas = new ObservableCollection<BaseResponseListItem>(areasTask.Result.Items!);

            //����������� ������
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

            //���������� ������
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
            
            //�������������, ��� �� ��� �������������
            Initialize = false;
        }
    }

    /// <summary>
    /// ������� ��������� ����
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void Gender_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //���� �������� �������, ������ ��� - �������
        if (GenderCheckBox.IsChecked) GenderLabel.Text = "�������";
        //����� - �������
        else GenderLabel.Text = "�������";
    }

    /// <summary>
    /// ������� ������ ����
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void Race_SelectedIndexChanged(object sender, EventArgs e)
    {
        //���� ��������� ������� ������, �������
        if (RacePicker.SelectedItem == null) return;

        //���� ��� �������������, �������
        if (Initialize) return;

        //��������� ������ ��������
        NationLoadActivityIndicator.IsRunning = true;
        NationStackLayout.IsVisible = false;
        PrefixNameStackLayout.IsVisible = false;

        //�������� ����� ������
        ErrorLabel.Text = null;

        try
        {
            //��������� ������� ������� ������ � �������
            if (_nations == null) throw new InnerException(Errors.EmptyServiceNations);

            //�������� ��������� ����
            long? raceId = ((BaseResponseListItem?)RacePicker.SelectedItem)?.Id;

            //�������� ��������� �����
            Nations = new ObservableCollection<BaseResponseListItem>((await _nations.GetNationsList(raceId)).Items!);

            //����������� ������
            NationPicker.ItemsSource = Nations;

            //������ ��������� ���������� ������ �����
            NationStackLayout.IsVisible = true;
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
            NationLoadActivityIndicator.IsRunning = false;
        }
    }

    /// <summary>
    /// ������� ������ �����
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void Nation_SelectedIndexChanged(object sender, EventArgs e)
    {
        //���� ��������� ������� ������, �������
        if (NationPicker.SelectedItem == null) return;

        //���� ��� �������������, �������
        if (Initialize) return;

        //��������� ������ ��������
        PrefixNameLoadActivityIndicator.IsRunning = true;
        PrefixNameStackLayout.IsVisible = false;

        //�������� ����� ������
        ErrorLabel.Text = null;

        try
        {
            //��������� ������� ������� ������ � ���������� ���
            if (_prefixNames == null) throw new InnerException(Errors.EmptyServicePrefixesNames);

            //�������� ��������� �����
            long? nationId = ((BaseResponseListItem?)NationPicker.SelectedItem)?.Id;

            //�������� ���������
            PrefixesNames = new ObservableCollection<BaseResponseListItem>((await _prefixNames.GetList(nationId)).Items!);

            //����������� ������
            PrefixNamePicker.ItemsSource = PrefixesNames;

            //������ ��������� ���������� ������ �����
            PrefixNameStackLayout.IsVisible = true;
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
            PrefixNameLoadActivityIndicator.IsRunning = false;
        }
    }

    /// <summary>
    /// ������� ��������� ������� �� ����� �����
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void GeneralBlockDecision_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //���� �������� �������, ������ ������� - �������
        if (GeneralBlockDecisionCheckBox.IsChecked) GeneralBlockDecisionLabel.Text = "�������";
        //����� - �� ������� � ������� �����������
        else
        {
            CommentOnGeneralBlockStackLayout.IsVisible = true;
            GeneralBlockDecisionLabel.Text = "�� �������";
        }
    }

    /// <summary>
    /// ������� ��������� ������� �� ����� ���� ��������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void BirthDateBlockDecision_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //���� �������� �������, ������ ������� - �������
        if (BirthDateBlockDecisionCheckBox.IsChecked) BirthDateBlockDecisionLabel.Text = "�������";
        //����� - �� ������� � ������� �����������
        else
        {
            CommentOnBirthDateBlockStackLayout.IsVisible = true;
            BirthDateBlockDecisionLabel.Text = "�� �������";
        }
    }

    /// <summary>
    /// ������� ������ ������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void Country_SelectedIndexChanged(object sender, EventArgs e)
    {
        //���� ��������� ������� ������, �������
        if (CountryPicker.SelectedItem == null) return;

        //���� ��� �������������, �������
        if (Initialize) return;

        //��������� ������ ��������
        RegionLoadActivityIndicator.IsRunning = true;
        RegionStackLayout.IsVisible = false;
        AreaStackLayout.IsVisible = false;

        //�������� ����� ������
        ErrorLabel.Text = null;

        try
        {
            //��������� ������� ������� ������ � ���������
            if (_regions == null) throw new InnerException(Errors.EmptyServiceRegions);

            //�������� ��������� ������
            long? countryId = ((BaseResponseListItem?)CountryPicker.SelectedItem)?.Id;

            //�������� ��������� ��������
            Regions = new ObservableCollection<BaseResponseListItem>((await _regions.GetRegionsList(countryId)).Items!);

            //����������� ������
            RegionPicker.ItemsSource = Regions;

            //������ ��������� ���������� ������ ��������
            RegionStackLayout.IsVisible = true;
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
            RegionLoadActivityIndicator.IsRunning = false;
        }
    }

    /// <summary>
    /// ������� ������ �������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void Region_SelectedIndexChanged(object sender, EventArgs e)
    {
        //���� ��������� ������� ������, �������
        if (RegionPicker.SelectedItem == null) return;

        //���� ��� �������������, �������
        if (Initialize) return;

        //��������� ������ ��������
        AreaLoadActivityIndicator.IsRunning = true;
        AreaStackLayout.IsVisible = false;

        //�������� ����� ������
        ErrorLabel.Text = null;

        try
        {
            //��������� ������� ������� ������ � ���������
            if (_areas == null) throw new InnerException(Errors.EmptyServiceAreas);

            //�������� ��������� ������
            long? regionId = ((BaseResponseListItem?)RegionPicker.SelectedItem)?.Id;

            //�������� ��������� ��������
            Areas = new ObservableCollection<BaseResponseListItem>((await _areas.GetAreasList(regionId, null)).Items!);

            //����������� ������
            AreaPicker.ItemsSource = Areas;

            //������ ��������� ���������� ������ ��������
            AreaStackLayout.IsVisible = true;
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
            AreaLoadActivityIndicator.IsRunning = false;
        }
    }

    /// <summary>
    /// ������� ��������� ������� �� ����� ��������������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void LocationBlockDecision_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //���� �������� �������, ������ ������� - �������
        if (LocationBlockDecisionCheckBox.IsChecked) LocationBlockDecisionLabel.Text = "�������";
        //����� - �� ������� � ������� �����������
        else
        {
            CommentOnLocationBlockStackLayout.IsVisible = true;
            LocationBlockDecisionLabel.Text = "�� �������";
        }
    }

    /// <summary>
    /// ������� ��������� ������� �� ����� ���������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void AppearanceBlockDecision_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //���� �������� �������, ������ ������� - �������
        if (AppearanceBlockDecisionCheckBox.IsChecked) AppearanceBlockDecisionLabel.Text = "�������";
        //����� - �� ������� � ������� �����������
        else
        {
            CommentOnAppearanceBlockStackLayout.IsVisible = true;
            AppearanceBlockDecisionLabel.Text = "�� �������";
        }
    }

    /// <summary>
    /// ������� ��������� ������� �� ����� �����������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void ImageBlockDecision_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //���� �������� �������, ������ ������� - �������
        if (ImageBlockDecisionCheckBox.IsChecked) ImageBlockDecisionLabel.Text = "�������";
        //����� - �� ������� � ������� �����������
        else
        {
            CommentOnImageBlockStackLayout.IsVisible = true;
            ImageBlockDecisionLabel.Text = "�� �������";
        }
    }
}