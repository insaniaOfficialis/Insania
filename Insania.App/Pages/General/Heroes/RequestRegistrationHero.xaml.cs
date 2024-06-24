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
    /// ��������� ������ � ����������� ����������
    /// </summary>
    private readonly IBiographiesHeroes? _biographiesHeroes;

    /// <summary>
    /// ��������� ������ � ����������� ������ �� ����������� ����������
    /// </summary>
    private readonly IBiographiesRequestsHeroesRegistration? _biographiesRequestsHeroesRegistration;


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
    /// ������ ��������� ���������
    /// </summary>
    private ObservableCollection<GetBiographiesHeroResponseListItem>? BiographiesHeroes { get; set; }

    /// <summary>
    /// �������� �������� ���������
    /// </summary>
    private List<BiographyElement> BiographyElements { get; set; }

    
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
        BiographyElements = [];

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
        _biographiesHeroes = App.Services?.GetService<IBiographiesHeroes>();
        _biographiesRequestsHeroesRegistration = App.Services?.GetService<IBiographiesRequestsHeroesRegistration>();
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
            if (_biographiesHeroes == null) throw new InnerException(Errors.EmptyServiceBiographiesHeroes);
            if (_biographiesRequestsHeroesRegistration == null) throw new InnerException(Errors.EmptyServiceBiographiesRequestsHeroesRegistration);

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
            Task<GetBiographiesHeroResponseList> biographiesTask = _biographiesHeroes.GetList(HeroResponse.Id);
            tasks.Add(biographiesTask);
            await Task.WhenAll(tasks);

            //�������� ������
            Nations = new ObservableCollection<BaseResponseListItem>(nationsTask.Result.Items!);
            PrefixesNames = new ObservableCollection<BaseResponseListItem>(prefixNamesTask.Result.Items!);
            Regions = new ObservableCollection<BaseResponseListItem>(regionsTask.Result.Items!);
            FileResponse = fileTask.Result;
            BiographiesHeroes = new ObservableCollection<GetBiographiesHeroResponseListItem>(biographiesTask.Result.Items!);

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
            if (RequestRegistrationHeroModel.GeneralBlockDecision != null)
            {
                GeneralBlockDecisionCheckBox.IsChecked = RequestRegistrationHeroModel.GeneralBlockDecision ?? true;
                GeneralBlockDecisionLabel.Text = RequestRegistrationHeroModel.GeneralBlockDecision == null ? "����������" : RequestRegistrationHeroModel.GeneralBlockDecision == true ? "�������" : "�� �������";
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
                BirthDateBlockDecisionLabel.Text = RequestRegistrationHeroModel.BirthDateBlockDecision == null ? "����������" : RequestRegistrationHeroModel.GeneralBlockDecision == true ? "�������" : "�� �������";
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
                LocationBlockDecisionLabel.Text = RequestRegistrationHeroModel.LocationBlockDecision == null ? "����������" : RequestRegistrationHeroModel.GeneralBlockDecision == true ? "�������" : "�� �������";
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
                AppearanceBlockDecisionLabel.Text = RequestRegistrationHeroModel.AppearanceBlockDecision == null ? "����������" : RequestRegistrationHeroModel.GeneralBlockDecision == true ? "�������" : "�� �������";
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
                ImageBlockDecisionLabel.Text = RequestRegistrationHeroModel.ImageBlockDecision == null ? "����������" : RequestRegistrationHeroModel.GeneralBlockDecision == true ? "�������" : "�� �������";
                if (RequestRegistrationHeroModel.ImageBlockDecision == false)
                {
                    CommentOnImageBlockStackLayout.IsVisible = true;
                    CommentOnImageBlockEntry.Text = RequestRegistrationHeroModel.CommentOnImageBlock;
                }
            }

            //������������ ���������
            foreach (var item in BiographiesHeroes)
            {
                var decision = await _biographiesRequestsHeroesRegistration.GetByUnique(item.Id, RequestRegistrationHeroModel.Id);
                AddBiography(item, decision);
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

    /// <summary>
    /// ������� ������� �� ������ ���������� ���������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void AddBiorgaphy_Clicked(object sender, EventArgs e)
    {
        //�������� ����� ������
        ErrorLabel.Text = null;

        try
        {
            //��������� ���������
            AddBiography();
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
    }
   
    /// <summary>
    /// ������� ��������� ������� �� ����� ���������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void BiographyDecision_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {

        //�������� �������
        foreach (var item in BiographyElements)
        {
            //���� �������� �������, ������ ������� - �������
            if (item.Decision!.IsChecked) item.DecisionText!.Text = "�������";
            //����� - �� ������� � ������� �����������
            else
            {
                item.Comment!.IsVisible = true;
                item.DecisionText!.Text = "�� �������";
            }
        }
    }

    /// <summary>
    /// ������� ������� �� ������ ����������� �� ���������� ��������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void Back_Clicked(object sender, EventArgs e)
    {
        ToBack();
    }

    /// <summary>
    /// ������� ������� ������ �����
    /// </summary>
    /// <returns></returns>
    protected override bool OnBackButtonPressed()
    {
        ToBack();
        return true;
    }


    /// <summary>
    /// ����� ���������� ����������� ���������
    /// </summary>
    private void AddBiography()
    {
        //���������, ��� ��������� ���� ��������� � �����������
        if (BiographyElements.Count > 0
            && (string.IsNullOrWhiteSpace(BiographyElements.Last().DayEnd?.Text)
                || BiographyElements.Last().MonthEnd?.SelectedIndex == null
                || string.IsNullOrWhiteSpace(BiographyElements.Last().CycleEnd?.Text)))
            throw new InnerException(Errors.NotExistsDateEndBiography);

        //������� �������� ������ ��� ���� ������ ����� ���������
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

        //�������� �����
        Application.Current!.Resources.TryGetValue("TitleSecondary", out var labelStyle);
        Application.Current!.Resources.TryGetValue("EntryPrimary", out var entryStyle);
        Application.Current!.Resources.TryGetValue("PrimaryText", out var boxViewColor);
        Application.Current!.Resources.TryGetValue("PickerPrimary", out var pickerStyle);


        //������ ���� ��� ������ � ��������� � ��������� ���������
        StackLayout dayBegin = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(dayBegin);

        //������ ���������� ��� ������ � ��������� � ���������
        Label dayBiographyBeginLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "���� ������*:",
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
            Placeholder = "������� ���� ������",
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


        //������ ���� ������ ������ � ��������� � ��������� ���������
        StackLayout monthBegin = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(monthBegin);

        //������ ���������� ������ ������ � ��������� � ���������
        Label monthBiographyBeginLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "����� ������*:",
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
            Title = "�������� ����� ������",
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


        //������ ���� ����� ������ � ��������� � ��������� ���������
        StackLayout cycleBegin = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(cycleBegin);

        //������ ���������� ����� ������ � ��������� � ���������
        Label cycleBiographyBeginLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "���� ������*:",
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
            Placeholder = "������� ���� ������",
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


        //������ ���� ��� ��������� � ��������� � ��������� ���������
        StackLayout dayEnd = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(dayEnd);

        //������ ���������� ��� ��������� � ��������� � ���������
        Label dayBiographyEndLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "���� ���������:",
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
            Placeholder = "������� ���� ���������",
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


        //������ ���� ������ ��������� � ��������� � ��������� ���������
        StackLayout monthEnd = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(monthEnd);

        //������ ���������� ������ ��������� � ��������� � ���������
        Label monthBiographyEndLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "����� ���������:",
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
            Title = "�������� ����� ���������",
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


        //������ ���� ����� ��������� � ��������� � ��������� ���������
        StackLayout cycleEnd = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(cycleEnd);

        //������ ���������� ����� ��������� � ��������� � ���������
        Label cycleBiographyEndLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "���� ���������:",
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
            Placeholder = "������� ���� ���������",
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


        //������ ���� ������ � ��������� � ��������� ���������
        StackLayout text = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(text);

        //������ ����� � ��������� � ���������
        Label textBiographyLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "����� ���������*:",
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
            Placeholder = "������� ����� ���������",
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

        //������ ����� �������� ������ �������� ��������� � ��������� ��� � ���������
        /*BiographyElement biographyElement = new(dayBiographyBeginEntry, monthBiographyBeginPicker, cycleBiographyBeginEntry,
            dayBiographyEndEntry, monthBiographyEndPicker, cycleBiographyEndEntry, textBiographyEditor);
        BiographyElements.Add(biographyElement);*/
    }

    /// <summary>
    /// ����� ���������� ����������� ��������� �� ���������
    /// </summary>
    /// <param name="request">������</param>
    /// <param name="decision">�������</param>
    /// <exception cref="Exception">�������������� ����������</exception>
    /// <exception cref="InnerException">������������ ����������</exception>
    private void AddBiography(GetBiographiesHeroResponseListItem? request, GetBiographyRequestHeroRegistrationResponse? decision)
    {
        //���������, ��� �������� ��������
        if (request == null) throw new InnerException(Errors.EmptyRequest);
        if (decision == null) throw new InnerException(Errors.EmtryDecision);

        //���������, ��� ��������� ���� ��������� � �����������
        if (BiographyElements.Count > 0
            && (string.IsNullOrWhiteSpace(BiographyElements.Last().DayEnd?.Text)
                || BiographyElements.Last().MonthEnd?.SelectedIndex == null
                || string.IsNullOrWhiteSpace(BiographyElements.Last().CycleEnd?.Text)))
            throw new InnerException(Errors.NotExistsDateEndBiography);

        //������� �������� ������ ��� ���� ������ ����� ���������
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

        //�������� �����
        Application.Current!.Resources.TryGetValue("TitleSecondary", out var labelStyle);
        Application.Current!.Resources.TryGetValue("EntryPrimary", out var entryStyle);
        Application.Current!.Resources.TryGetValue("PrimaryText", out var boxViewColor);
        Application.Current!.Resources.TryGetValue("PickerPrimary", out var pickerStyle);
        Application.Current!.Resources.TryGetValue("CheckBoxPrimary", out var checkBoxStyle);


        //������ ��������� �����
        Label numberTitleLabel = new()
        {
            HorizontalOptions = LayoutOptions.Center,
            Text = (BiographyElements.Count + 1).ToString(),
            Style = (Style)labelStyle
        };
        BiographyStackLayout.Add(numberTitleLabel);


        //������ ���� ��� ������ � ��������� � ��������� ���������
        StackLayout dayBegin = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(dayBegin);

        //������ ���������� ��� ������ � ��������� � ���������
        Label dayBiographyBeginLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "���� ������*:",
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
            Placeholder = "������� ���� ������",
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


        //������ ���� ������ ������ � ��������� � ��������� ���������
        StackLayout monthBegin = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(monthBegin);

        //������ ���������� ������ ������ � ��������� � ���������
        Label monthBiographyBeginLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "����� ������*:",
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
            Title = "�������� ����� ������",
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


        //������ ���� ����� ������ � ��������� � ��������� ���������
        StackLayout cycleBegin = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(cycleBegin);

        //������ ���������� ����� ������ � ��������� � ���������
        Label cycleBiographyBeginLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "���� ������*:",
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
            Placeholder = "������� ���� ������",
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


        //������ ���� ��� ��������� � ��������� � ��������� ���������
        StackLayout dayEnd = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(dayEnd);

        //������ ���������� ��� ��������� � ��������� � ���������
        Label dayBiographyEndLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "���� ���������:",
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
            Placeholder = "������� ���� ���������",
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


        //������ ���� ������ ��������� � ��������� � ��������� ���������
        StackLayout monthEnd = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(monthEnd);

        //������ ���������� ������ ��������� � ��������� � ���������
        Label monthBiographyEndLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "����� ���������:",
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
            Title = "�������� ����� ���������",
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


        //������ ���� ����� ��������� � ��������� � ��������� ���������
        StackLayout cycleEnd = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(cycleEnd);

        //������ ���������� ����� ��������� � ��������� � ���������
        Label cycleBiographyEndLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "���� ���������:",
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
            Placeholder = "������� ���� ���������",
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


        //������ ���� ������ � ��������� � ��������� ���������
        StackLayout text = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(text);

        //������ ����� � ��������� � ���������
        Label textBiographyLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "����� ���������*:",
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
            Placeholder = "������� ����� ���������",
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

        //������ ���� �������
        Label decisionTitleLabel = new()
        {
            HorizontalOptions = LayoutOptions.Center,
            Text = "�������",
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
            Text = "�������:",
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
            Text = decision.Decision == null ? "����������" : decision.Decision == true ? "�������" : "�� �������",
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
            Text = "�����������*:",
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
            Placeholder = "������� �����������",
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

        //������ ����� �������� ������ �������� ��������� � ��������� ��� � ���������
        BiographyElement biographyElement = new(dayBiographyBeginEntry, monthBiographyBeginPicker, cycleBiographyBeginEntry,
            dayBiographyEndEntry, monthBiographyEndPicker, cycleBiographyEndEntry, textBiographyEditor, decisionCheckBox,
            decisionTextLabel, commentEntry);
        BiographyElements.Add(biographyElement);
    }

    /// <summary>
    /// ����� �������� �� ���������� ��������
    /// </summary>
    private async void ToBack()
    {
        //��������� �� �������� �����������
        await Navigation.PushModalAsync(new NavigationPage(new Authentication())
        {
            BarBackgroundColor = Color.FromArgb("#FF272727"),
            BarTextColor = Colors.Transparent
        });
    }
}