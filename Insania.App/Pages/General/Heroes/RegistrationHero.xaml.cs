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
    /// ����������� ������ �������� ����������� ���������
    /// </summary>
    public RegistrationHero(AddUserRequest addUserRequest)
    {
        //�������������� ����������
        InitializeComponent();

        //�������� �������
        _checkConnection = App.Services?.GetService<ICheckConnection>();
        _races = App.Services?.GetService<IRaces>();
        _nations = App.Services?.GetService<INations>();
        _months = App.Services?.GetService<IMonths>();
        _countries = App.Services?.GetService<ICountries>();
        _regions = App.Services?.GetService<IRegions>();
        _areas = App.Services?.GetService<IAreas>();

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
            //��������� ������� ������� �������� ����������
            if (_checkConnection == null) throw new InnerException(Errors.EmptyServiceCheckConnection);

            //���������, ��� ������������ � �������
            if (!await _checkConnection.CheckNotAuthorize()) throw new InnerException(Errors.NoConnection);

            //��������� ������� ������� ������ � ������
            if (_races == null) throw new InnerException(Errors.EmptyServiceRaces);

            //�������� ��������� ���
            Races = new ObservableCollection<BaseResponseListItem>((await _races.GetRacesList()).Items!);

            //����������� ������
            RacePicker.ItemsSource = Races;

            //��������� ������� ������� ������ � ��������
            if (_months == null) throw new InnerException(Errors.EmptyServiceMonths);

            //�������� ��������� �������
            Months = new ObservableCollection<BaseResponseListItem>((await _months.GetMonthsList()).Items!);

            //����������� ������
            BirtMonthPicker.ItemsSource = Months;

            //��������� ������� ������� ������ �� ��������
            if (_countries == null) throw new InnerException(Errors.EmptyService�ountries);

            //�������� ��������� �����
            Countries = new ObservableCollection<BaseResponseListItem>((await _countries.GetCountriesList()).Items!);

            //����������� ������
            CountryPicker.ItemsSource = Countries;
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
    /// ������� ������� �� ������ ����������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void Save_Clicked(object sender, EventArgs e)
    {

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
}