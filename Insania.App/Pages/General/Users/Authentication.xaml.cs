using Insania.App.Pages.Desktop.OutCategories;
using Insania.App.Pages.General.Heroes;
using Insania.BusinessLogic.OutOfCategories.CheckConnection;
using Insania.BusinessLogic.Users.Authentication;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Models.Users.Authentication;

namespace Insania.App.Pages.General.Users;

/// <summary>
/// �������� ��������������
/// </summary>
public partial class Authentication : ContentPage
{
    /// <summary>
    /// ��������� �������� ����������
    /// </summary>
    private readonly ICheckConnection? _checkConnection;

    /// <summary>
    /// ��������� ������� ��������������
    /// </summary>
    private readonly IAuthentication? _authentication;

    /// <summary>
    /// ����������� �������� ��������������
    /// </summary>
    public Authentication()
	{
        SecureStorage.Default.Remove("token");

        //�������������� ����������
        InitializeComponent();

        //�������� �������
        _checkConnection = App.Services?.GetService<ICheckConnection>();
        _authentication = App.Services?.GetService<IAuthentication>();
    }

    /// <summary>
    /// ������� �������� ����
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        //��������� ���������� ������ �������� ����������
        bool checkConnection = false;

        //��������� ������ ��������
        LoadActivityIndicator.IsRunning = true;
        AuthenticationStackLayout.IsVisible = false;
        FeedbackButton.IsVisible = false;

        try
        {
            //��������� ������� ������� �������� ����������
            if (_checkConnection == null) throw new InnerException(Errors.EmptyServiceCheckConnection);

            //���������, ��� ������������ � �������
            if (!await _checkConnection.CheckNotAuthorize()) throw new InnerException(Errors.NoConnection);

            //���� ���� ����� � ���������� �������������� �� ����, ��������� �� �������
            if (!string.IsNullOrWhiteSpace(await SecureStorage.Default.GetAsync("token"))) checkConnection = await _checkConnection.CheckNotAuthorize();
        }
        catch(InnerException ex)
        {
            //������������� ����� ������
            ErrorLabel.Text = ex.Message;
        }
        catch(Exception ex)
        {
            //������������� ����� ������
            ErrorLabel.Text = ex.Message;
        }
        finally
        {
            //������������� ������ ��������
            LoadActivityIndicator.IsRunning = false;

            //���������� ��������� ���������
            AuthenticationStackLayout.IsVisible = true;
            FeedbackButton.IsVisible = true;
        }

        //���� �������� ���������� ������ �������, ��������� �� �������
        if (checkConnection) ToMain(null, null);
    }

    /// <summary>
    /// ������� ������� �� ������ �����������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void Authorize_Clicked(object sender, EventArgs e)
    {
        //��������� ���������� ������ �������������
        AuthenticationResponse? result = new();

        //��������� ������ ��������
        LoadActivityIndicator.IsRunning = true;
        AuthenticationStackLayout.IsVisible = false;
        FeedbackButton.IsVisible = false;

        try
        {
            //������� ����������
            LoginEntry.IsEnabled = false;
            LoginEntry.IsEnabled = true;
            PasswordEntry.IsEnabled = false;
            PasswordEntry.IsEnabled = true;

            //�������� ����� ������
            ErrorLabel.Text = null;

            //�������� ����� �����������
            if (_authentication == null) throw new InnerException(Errors.EmptyServiceAuthentication);
            result = await _authentication.Login(LoginEntry.Text, PasswordEntry.Text);
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
            AuthenticationStackLayout.IsVisible = true;
            FeedbackButton.IsVisible = true;
        }

        //���� �������� �����, ��������� �� ������� ��������
        if (!string.IsNullOrWhiteSpace(result.Token)) ToMain(null, null);
    }

    /// <summary>
    /// ������� ������� �� ������ �������������� ������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void RestorePassword_Clicked(object sender, EventArgs e)
    {
        //������� �� �������� �������������� ������
        ToRestrorePassword(null, null);
    }

    /// <summary>
    /// ������� ������� �� ������ �����������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private void Registration_Clicked(object sender, EventArgs e)
    {
        //������� �� �������� �����������
        ToRegistration(null, null);
    }

    /// <summary>
    /// ������� ������� �� ������ �������� �����
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void Feedback_Clicked(object sender, EventArgs e)
    {
        await Task.Delay(500);
    }

    /// <summary>
    /// ������� ������� ������ �����
    /// </summary>
    /// <returns></returns>
    protected override bool OnBackButtonPressed()
    {
        return true;
    }

    /// <summary>
    /// ����� �������� �� ������� ��������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void ToMain(object? sender, EventArgs? e)
    {
        if (DeviceInfo.Idiom == DeviceIdiom.Desktop) await Navigation.PushModalAsync(new MainDesktop());
    }

    /// <summary>
    /// ����� �������� �� �������� �����������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void ToRegistration(object? sender, EventArgs? e)
    {
        //��������� �� ����� ��������
        await Navigation.PushAsync(new RegistrationUser());
    }

    /// <summary>
    /// ����� �������� �� �������� �������������� ������
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void ToRestrorePassword(object? sender, EventArgs? e)
    {
        //��������� �� ����� ��������
        await Navigation.PushAsync(new RequestRegistrationHero(1));
    }
}