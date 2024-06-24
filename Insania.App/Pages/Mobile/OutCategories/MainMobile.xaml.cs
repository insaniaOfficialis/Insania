using System.Collections.ObjectModel;

using Insania.BusinessLogic.Files.Files;
using Insania.BusinessLogic.Heroes.Heroes;
using Insania.BusinessLogic.OutOfCategories.CheckConnection;
using Insania.BusinessLogic.Users.Users;
using Insania.Models.Files.Files;
using Insania.Models.Heroes.Heroes;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.App.Pages.Mobile.OutCategories;

/// <summary>
/// ����� ������� �������� ��� ��������� ���������
/// </summary>
public partial class MainMobile : ContentPage
{
    /// <summary>
    /// ��������� �������� ����������
    /// </summary>
    private readonly ICheckConnection? _checkConnection;

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
    /// ���������
    /// </summary>
    private ObservableCollection<GetHeroesResponseListItem>? Heroes { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    private GetHeroResponse? HeroResponse { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    private GetFileResponse? FileResponse { get; set; }

    /// <summary>
    /// ����������� ������� �������� ��� ��������� ���������
    /// </summary>
    public MainMobile()
    {
        //�������������� ����������
        InitializeComponent();

        //�������� �������
        _checkConnection = App.Services?.GetService<ICheckConnection>();
        _users = App.Services?.GetService<IUsers>();
        _heroes = App.Services?.GetService<IHeroes>();
        _files = App.Services?.GetService<IFiles>();
    }

    /// <summary>
    /// ������� �������� ����
    /// </summary>
    /// <param name="sender">�����������</param>
    /// <param name="e">�������</param>
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        //��������� ������ ��������
        LoadStackLayout.IsVisible = true;
        LoadActivityIndicator.IsRunning = true;
        MainDesktopGrid.IsVisible = false;

        try
        {

            //��������� ������� ��������
            if (_checkConnection == null) throw new InnerException(Errors.EmptyServiceCheckConnection);
            if (_users == null) throw new InnerException(Errors.EmptyServiceUsers);
            if (_heroes == null) throw new InnerException(Errors.EmptyServiceHeroes);
            if (_files == null) throw new InnerException(Errors.EmptyServiceFiles);

            //��������� ����������
            if (!await _checkConnection.CheckAuthorize()) throw new InnerException(Errors.NoConnection);

            //�������� ������ ����������
            if (_heroes == null) throw new InnerException(Errors.EmptyServiceHeroes);
            var heroes = await _heroes.GetListByCurrent(null);
            if (heroes == null || heroes.Items == null || heroes.Items.Count < 1 || heroes.Items.Where(x => x.IsCurrent == true).Count() != 1) throw new InnerException(Errors.EmptyHeroes);
            Heroes = new ObservableCollection<GetHeroesResponseListItem>(heroes.Items);
            HeroPicker.ItemsSource = Heroes;
            HeroPicker.SelectedItem = Heroes.First(x => x.IsCurrent == true);

            //�������� ������ �� �������� ���������
            HeroResponse = await _heroes.GetById(Heroes.First(x => x.IsCurrent == true).Id);

            //�������� ���� ���������
            FileResponse = await _files.GetById(HeroResponse.FileId);
            FileResponse.Stream!.Position = 0;
            HeroImage.Source = ImageSource.FromStream(() => FileResponse.Stream);
        }
        catch (InnerException ex)
        {
            //������������� ����� ������
            await DisplayAlert(Errors.Known, string.Format("{0} {1}", Errors.Error, ex), "�K");
        }
        catch (Exception ex)
        {
            //������������� ����� ������
            await DisplayAlert(Errors.Unknown, string.Format("{0} {1}", Errors.Error, ex), "�K");
        }
        finally
        {
            //������������� ������ ��������
            LoadStackLayout.IsVisible = false;
            LoadActivityIndicator.IsRunning = false;

            //���������� ��������� ���������
            MainDesktopGrid.IsVisible = true;
        }
    }
}