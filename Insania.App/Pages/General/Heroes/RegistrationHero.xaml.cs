using System.Collections.ObjectModel;

using Insania.BusinessLogic.Appearance.EyesColors;
using Insania.BusinessLogic.Appearance.HairsColors;
using Insania.BusinessLogic.Appearance.TypesBodies;
using Insania.BusinessLogic.Appearance.TypesFaces;
using Insania.BusinessLogic.Biology.Nations;
using Insania.BusinessLogic.Biology.Races;
using Insania.BusinessLogic.Chronology.Months;
using Insania.BusinessLogic.OutOfCategories.CheckConnection;
using Insania.BusinessLogic.Politics.Areas;
using Insania.BusinessLogic.Politics.Countries;
using Insania.BusinessLogic.Politics.Regions;
using Insania.BusinessLogic.Users.Users;
using Insania.Models.Heroes.BiographiesHeroes;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Models.Users.Users;

namespace Insania.App.Pages.General.Heroes;

/// <summary>
/// ����� �������� ����������� ����������
/// </summary>
public partial class RegistrationHero : ContentPage
{
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
    /// ��������� ������ � ������ ����
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
    /// ������ ���������� ������������
    /// </summary>
    private readonly AddUserRequest? _addUserRequest;

    /// <summary>
    /// ������ ���
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Races { get; set; }

    /// <summary>
    /// ������ �����
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Nations { get; set; }

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
    /// �������� �������� ���������
    /// </summary>
    private List<BiographyElement> BiographyElements {  get; set; }


    /// <summary>
    /// ����������� ������ �������� ����������� ���������
    /// </summary>
    public RegistrationHero(AddUserRequest addUserRequest)
    {
        //�������������� ����������
        InitializeComponent();
        BiographyElements = [];

        //�������� �������
        _checkConnection = App.Services?.GetService<ICheckConnection>();
        _races = App.Services?.GetService<IRaces>();
        _nations = App.Services?.GetService<INations>();
        _months = App.Services?.GetService<IMonths>();
        _countries = App.Services?.GetService<ICountries>();
        _regions = App.Services?.GetService<IRegions>();
        _areas = App.Services?.GetService<IAreas>();
        _typesBodies = App.Services?.GetService<ITypesBodies>();
        _typesFaces = App.Services?.GetService<ITypesFaces>();
        _hairsColors = App.Services?.GetService<IHairsColors>();
        _eyesColors = App.Services?.GetService<IEyesColors>();
        _users = App.Services?.GetService<IUsers>();

        //���������� �������� ���������
        _addUserRequest = addUserRequest;
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
        RegistrationHeroStackLayout.IsVisible = false;

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
            await Task.WhenAll(tasks);

            //�������� ��������
            Races = new ObservableCollection<BaseResponseListItem>(racesTask.Result.Items!);
            Months = new ObservableCollection<BaseResponseListItem>(monthsTask.Result.Items!);
            Countries = new ObservableCollection<BaseResponseListItem>(countriesTask.Result.Items!);
            TypesBodies = new ObservableCollection<BaseResponseListItem>(typesBodiesTask.Result.Items!);
            TypesFaces = new ObservableCollection<BaseResponseListItem>(typesFacesTask.Result.Items!);
            HairsColors = new ObservableCollection<BaseResponseListItem>(hairsColorsTask.Result.Items!);
            EyesColors = new ObservableCollection<BaseResponseListItem>(eyesColorsTask.Result.Items!);

            //����������� ������
            RacePicker.ItemsSource = Races;
            MonthBirtPicker.ItemsSource = Months;
            CountryPicker.ItemsSource = Countries;
            TypeBodyPicker.ItemsSource = TypesBodies;
            TypeFacePicker.ItemsSource = TypesFaces;
            HairsColorPicker.ItemsSource = HairsColors;
            EyesColorPicker.ItemsSource = EyesColors;

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
        finally
        {
            //������������� ������ ��������
            LoadActivityIndicator.IsVisible = false;
            LoadActivityIndicator.IsRunning = false;

            //���������� ��������� ���������
            RegistrationHeroStackLayout.IsVisible = true;
        }
    }

    /// <summary>
    /// ������� ������� �� ������ ����������� �� ���������� ��������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void Back_Clicked(object sender, EventArgs e)
    {
        ToBack(null, null);
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
        if (RacePicker.SelectedItem ==  null) return;

        //��������� ������ ��������
        NationLoadActivityIndicator.IsRunning = true;
        NationStackLayout.IsVisible = false;

        //�������� ����� ������
        ErrorLabel.Text = null;

        try
        {
            //��������� ������� ������� ������ � �������
            if (_nations == null) throw new InnerException(Errors.EmptyServiceNations);

            //�������� ��������� ����
            long? raceId = ((BaseResponseListItem?) RacePicker.SelectedItem)?.Id;

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
    /// ������� ������ ������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void Country_SelectedIndexChanged(object sender, EventArgs e)
    {
        //���� ��������� ������� ������, �������
        if (CountryPicker.SelectedItem == null) return;

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
    /// ������� ������� �� ������ �������� �����������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void Download_Clicked(object sender, EventArgs e)
    {
        try
        {
            //��������� ����� ���� �������� ����������
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "�������� �����������",
                FileTypes = FilePickerFileType.Images
            });

            //�������, ���� �� ������� �����������
            if (result == null) return;

            //��������� �����
            var stream = await result.OpenReadAsync();

            //���������� ����� � �������� �����������
            HeroImage.Source = ImageSource.FromStream(() => stream);
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
    /// ������� ������� �� ������ ���������� ���������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void AddBiorgaphy_Clicked(object sender, EventArgs e)
    {
        //��������� ���������
        AddBiography();
    }

    /// <summary>
    /// ������� ������� �� ������ ����������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void Save_Clicked(object sender, EventArgs e)
    {
        //��������� ������ ��������
        LoadActivityIndicator.IsRunning = true;
        RegistrationHeroStackLayout.IsVisible = false;

        try
        {
            //�������� ����� ������
            ErrorLabel.Text = null;

            //������������ ������
            //var user = await _users!.AddUser(_addUserRequest);

            //������������ ���������


            List<AddBiographyHeroRequest> biographiesHero = [];
            foreach (var item in BiographyElements)
            {
                AddBiographyHeroRequest biographyHero = new(Convert.ToInt32(item.DayBegin!.Text),
                    ((BaseResponseListItem)item.MonthBegin!.SelectedItem).Id, Convert.ToInt32(item.CycleBegin!.Text),
                    Convert.ToInt32(item.DayEnd!.Text), ((BaseResponseListItem)item.MonthEnd!.SelectedItem).Id,
                    Convert.ToInt32(item.CycleEnd!.Text), item.Text!.Text);

                biographiesHero.Add(biographyHero);
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
            LoadActivityIndicator.IsRunning = false;

            //���������� ��������� ���������
            RegistrationHeroStackLayout.IsVisible = true;
        }
    }

    /// <summary>
    /// ����� �������� �� ���������� ��������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void ToBack(object? sender, EventArgs? e)
    {
        //��������� �� ������ ��������
        await Navigation.PopAsync();
    }

    /// <summary>
    /// ����� ���������� ����������� ���������
    /// </summary>
    private void AddBiography()
    {
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
            Text = "���� ���������",
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
            Text = "����� ���������*:",
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
            Text = "���� ���������*:",
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
            VerticalOptions = LayoutOptions.Fill,
#if WINDOWS
            WidthRequest = 400,
#else
            WidthRequest = 280,
#endif
            Placeholder = "������� ����� ���������",
            AutoSize = EditorAutoSizeOption.TextChanges,
            Style = (Style)entryStyle
        };
        textBiographyScroolView.Content = textBiographyEditor;

        //������ ����� �������� ������ �������� ��������� � ��������� ��� � ���������
        BiographyElement biographyElement = new(dayBiographyBeginEntry, monthBiographyBeginPicker, cycleBiographyBeginEntry,
            dayBiographyEndEntry, monthBiographyEndPicker, cycleBiographyEndEntry, textBiographyEditor);
        BiographyElements.Add(biographyElement);
    }

    /// <summary>
    /// ����� �������� ���������
    /// </summary>
    private class BiographyElement(Entry dayBegin, Picker monthBegin, Entry cycleBegin, Entry dayEnd, Picker monthEnd,
        Entry cycleEnd, Editor text)
    {
        /// <summary>
        /// ���� ����� ���� ������
        /// </summary>
        public Entry? DayBegin { get; set; } = dayBegin;

        /// <summary>
        /// ���������� ������ ������� ������
        /// </summary>
        public Picker? MonthBegin { get; set; } = monthBegin;

        /// <summary>
        /// ���� ����� ����� ������
        /// </summary>
        public Entry? CycleBegin { get; set; } = cycleBegin;

        /// <summary>
        /// ���� ����� ���� ���������
        /// </summary>
        public Entry? DayEnd { get; set; } = dayEnd;

        /// <summary>
        /// ���������� ������ ������� ���������
        /// </summary>
        public Picker? MonthEnd { get; set; } = monthEnd;

        /// <summary>
        /// ���� ����� ���� ���������
        /// </summary>
        public Entry? CycleEnd { get; set; } = cycleEnd;

        /// <summary>
        /// ���� ����� ����� ���������
        /// </summary>
        public Editor? Text { get; set; } = text;
    }
}