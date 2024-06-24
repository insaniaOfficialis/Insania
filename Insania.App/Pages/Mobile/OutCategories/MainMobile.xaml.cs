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
/// Класс главной страницы для мобильных устройств
/// </summary>
public partial class MainMobile : ContentPage
{
    /// <summary>
    /// Интерфейс проверки соединения
    /// </summary>
    private readonly ICheckConnection? _checkConnection;

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
    /// Персонажи
    /// </summary>
    private ObservableCollection<GetHeroesResponseListItem>? Heroes { get; set; }

    /// <summary>
    /// Персонаж
    /// </summary>
    private GetHeroResponse? HeroResponse { get; set; }

    /// <summary>
    /// Файл
    /// </summary>
    private GetFileResponse? FileResponse { get; set; }

    /// <summary>
    /// Конструктор главной страницы для мобильных устройств
    /// </summary>
    public MainMobile()
    {
        //Инициализируем компоненты
        InitializeComponent();

        //Получаем сервисы
        _checkConnection = App.Services?.GetService<ICheckConnection>();
        _users = App.Services?.GetService<IUsers>();
        _heroes = App.Services?.GetService<IHeroes>();
        _files = App.Services?.GetService<IFiles>();
    }

    /// <summary>
    /// Событие загрузки окна
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        //Запускаем колесо загрузки
        LoadStackLayout.IsVisible = true;
        LoadActivityIndicator.IsRunning = true;
        MainDesktopGrid.IsVisible = false;

        try
        {

            //Проверяем наличие сервисов
            if (_checkConnection == null) throw new InnerException(Errors.EmptyServiceCheckConnection);
            if (_users == null) throw new InnerException(Errors.EmptyServiceUsers);
            if (_heroes == null) throw new InnerException(Errors.EmptyServiceHeroes);
            if (_files == null) throw new InnerException(Errors.EmptyServiceFiles);

            //Проверяем соединение
            if (!await _checkConnection.CheckAuthorize()) throw new InnerException(Errors.NoConnection);

            //Получаем список персонажей
            if (_heroes == null) throw new InnerException(Errors.EmptyServiceHeroes);
            var heroes = await _heroes.GetListByCurrent(null);
            if (heroes == null || heroes.Items == null || heroes.Items.Count < 1 || heroes.Items.Where(x => x.IsCurrent == true).Count() != 1) throw new InnerException(Errors.EmptyHeroes);
            Heroes = new ObservableCollection<GetHeroesResponseListItem>(heroes.Items);
            HeroPicker.ItemsSource = Heroes;
            HeroPicker.SelectedItem = Heroes.First(x => x.IsCurrent == true);

            //Получаем данные по текущему персонажу
            HeroResponse = await _heroes.GetById(Heroes.First(x => x.IsCurrent == true).Id);

            //Получаем файл персонажа
            FileResponse = await _files.GetById(HeroResponse.FileId);
            FileResponse.Stream!.Position = 0;
            HeroImage.Source = ImageSource.FromStream(() => FileResponse.Stream);
        }
        catch (InnerException ex)
        {
            //Устанавливаем текст ошибки
            await DisplayAlert(Errors.Known, string.Format("{0} {1}", Errors.Error, ex), "ОK");
        }
        catch (Exception ex)
        {
            //Устанавливаем текст ошибки
            await DisplayAlert(Errors.Unknown, string.Format("{0} {1}", Errors.Error, ex), "ОK");
        }
        finally
        {
            //Останавливаем колесо загрузки
            LoadStackLayout.IsVisible = false;
            LoadActivityIndicator.IsRunning = false;

            //Возвращаем видимость элементов
            MainDesktopGrid.IsVisible = true;
        }
    }
}