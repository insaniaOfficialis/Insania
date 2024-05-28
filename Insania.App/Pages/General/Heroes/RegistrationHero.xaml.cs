using System.Collections.ObjectModel;

using Insania.BusinessLogic.Biology.Races;
using Insania.BusinessLogic.OutOfCategories.CheckConnection;
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
    /// ������ ���������� ������������
    /// </summary>
    private readonly AddUserRequest? _addUserRequest;

    /// <summary>
    /// ������ ���
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Races { get; set; }

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
    /// ������� ������� �� ������ ����������� �� ���������� ��������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void Back_Clicked(object sender, EventArgs e)
    {
        ToBack(null, null);
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
    /// ������� ������ ����
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void Race_SelectedIndexChanged(object sender, EventArgs e)
    {

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