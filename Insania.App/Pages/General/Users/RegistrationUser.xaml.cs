using Insania.App.Logic.OutCategories.CheckConnection;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Models.Users.Authentication;
using Insania.Models.Users.Users;

namespace Insania.App.Pages.General.Users;

/// <summary>
/// ����� �������� ����������� �������������
/// </summary>
public partial class RegistrationUser : ContentPage
{
    /// <summary>
    /// ��������� ������� �������� ����������
    /// </summary>
    private readonly ICheckConnection? _checkConnection;

    /// <summary>
    /// ����������� ������ �������� ����������� �������������
    /// </summary>
	public RegistrationUser()
    {
        try
        {
            //�������������� ����������
            InitializeComponent();

            //�������� �������
            _checkConnection = App.Services?.GetService<ICheckConnection>();
        }
        catch { }
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
        RegistrationUserStackLayout.IsVisible = false;

        try
        {
            //��������� ������� ������� �������� ����������
            if (_checkConnection == null) throw new InnerException(Errors.EmptyServiceCheckConnection);

            //���������, ��� ������������ � �������
            if (!await _checkConnection.CheckNotAuthorize()) throw new InnerException(Errors.NoConnection);
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
            RegistrationUserStackLayout.IsVisible = true;
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
    /// ������� ������� �� ������ �����
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void Next_Clicked(object sender, EventArgs e)
    {
        //��������� ������ ��������
        LoadActivityIndicator.IsRunning = true;
        RegistrationUserStackLayout.IsVisible = false;

        try
        {
            //������� ����������
            LoginEntry.IsEnabled = false;
            LoginEntry.IsEnabled = true;
            PasswordEntry.IsEnabled = false;
            PasswordEntry.IsEnabled = true;
            LastNameEntry.IsEnabled = false;
            LastNameEntry.IsEnabled = true;
            FirstNameEntry.IsEnabled = false;
            FirstNameEntry.IsEnabled = true;
            PatronymicEntry.IsEnabled = false;
            PatronymicEntry.IsEnabled = true;
            BirthDateDatePicker.IsEnabled = false;
            BirthDateDatePicker.IsEnabled = true;
            PhoneNumberEntry.IsEnabled = false;
            PhoneNumberEntry.IsEnabled = true;
            EmailEntry.IsEnabled = false;
            EmailEntry.IsEnabled = true;
            LinkVKEntry.IsEnabled = false;
            LinkVKEntry.IsEnabled = true;

            //�������� ����� ������
            ErrorLabel.Text = null;

            //������ ������ ������� ������������
            AddUserRequest addUserRequest = new(LoginEntry.Text, PasswordEntry.Text, LastNameEntry.Text, FirstNameEntry.Text,
                PatronymicEntry.Text, GenderCheckBox.IsChecked, BirthDateDatePicker.Date, PhoneNumberEntry.Text, EmailEntry.Text,
                LinkVKEntry.Text);
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
            RegistrationUserStackLayout.IsVisible = true;
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
    /// ����� �������� �� ���������� ��������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void ToRegistrationPlayer(object? sender, EventArgs? e)
    {
        //��������� �� ������ ��������
        await Navigation.PopAsync();
    }
}