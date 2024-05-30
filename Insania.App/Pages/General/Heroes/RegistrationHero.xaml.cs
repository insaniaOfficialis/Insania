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
/// Класс страницы регистрации персонажей
/// </summary>
public partial class RegistrationHero : ContentPage
{
    /// <summary>
    /// Интерфейс проверки соединения
    /// </summary>
    private readonly ICheckConnection? _checkConnection;

    /// <summary>
    /// Интерфейс работы с расами
    /// </summary>
    private readonly IRaces? _races;

    /// <summary>
    /// Интерфейс работы с нациями
    /// </summary>
    private readonly INations? _nations;

    /// <summary>
    /// Интерфейс работы с месяцами
    /// </summary>
    private readonly IMonths? _months;

    /// <summary>
    /// Интерфейс работы со странами
    /// </summary>
    private readonly ICountries? _countries;

    /// <summary>
    /// Интерфейс работы с регионами
    /// </summary>
    private readonly IRegions? _regions;

    /// <summary>
    /// Интерфейс работы с областями
    /// </summary>
    private readonly IAreas? _areas;

    /// <summary>
    /// Интерфейс работы с типами телосложений
    /// </summary>
    private readonly ITypesBodies? _typesBodies;

    /// <summary>
    /// Интерфейс работы с типами лицы
    /// </summary>
    private readonly ITypesFaces? _typesFaces;

    /// <summary>
    /// Интерфейс работы с цветами волос
    /// </summary>
    private readonly IHairsColors? _hairsColors;

    /// <summary>
    /// Интерфейс работы с цветами глаз
    /// </summary>
    private readonly IEyesColors? _eyesColors;

    /// <summary>
    /// Интерфейс работы с пользователями
    /// </summary>
    private readonly IUsers? _users;


    /// <summary>
    /// Модель добавления пользователя
    /// </summary>
    private readonly AddUserRequest? _addUserRequest;

    /// <summary>
    /// Список рас
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Races { get; set; }

    /// <summary>
    /// Список наций
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Nations { get; set; }

    /// <summary>
    /// Список месяцев
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Months { get; set; }

    /// <summary>
    /// Список стран
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Countries { get; set; }

    /// <summary>
    /// Список регионов
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Regions { get; set; }

    /// <summary>
    /// Список областей
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? Areas { get; set; }

    /// <summary>
    /// Список типов телосложений
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? TypesBodies { get; set; }

    /// <summary>
    /// Список типов лиц
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? TypesFaces { get; set; }

    /// <summary>
    /// Список цветов волос
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? HairsColors { get; set; }

    /// <summary>
    /// Список цветов глаз
    /// </summary>
    private ObservableCollection<BaseResponseListItem>? EyesColors { get; set; }

    /// <summary>
    /// Коллеция элемнтов биографий
    /// </summary>
    private List<BiographyElement> BiographyElements {  get; set; }


    /// <summary>
    /// Конструктор класса страницы регистрации персонажа
    /// </summary>
    public RegistrationHero(AddUserRequest addUserRequest)
    {
        //Инициализируем компоненты
        InitializeComponent();
        BiographyElements = [];

        //Получаем сервисы
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

        //Записываем входящие параметры
        _addUserRequest = addUserRequest;
    }

    /// <summary>
    /// Событие загрузки окна
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        //Запускаем колесо загрузки
        LoadActivityIndicator.IsVisible = true;
        LoadActivityIndicator.IsRunning = true;
        RegistrationHeroStackLayout.IsVisible = false;

        try
        {
            //Проверяем наличие сервисов
            if (_checkConnection == null) throw new InnerException(Errors.EmptyServiceCheckConnection);
            if (_races == null) throw new InnerException(Errors.EmptyServiceRaces);
            if (_months == null) throw new InnerException(Errors.EmptyServiceMonths);
            if (_countries == null) throw new InnerException(Errors.EmptyServiceСountries);
            if (_typesBodies == null) throw new InnerException(Errors.EmptyServiceTypesBodies);
            if (_typesFaces == null) throw new InnerException(Errors.EmptyServiceTypesFaces);
            if (_hairsColors == null) throw new InnerException(Errors.EmptyServiceHairsColors);
            if (_eyesColors == null) throw new InnerException(Errors.EmptyServiceEyesColors);
            if (_users == null) throw new InnerException(Errors.EmptyServiceUsers);

            //Проверяем соединение
            if (!await _checkConnection.CheckNotAuthorize()) throw new InnerException(Errors.NoConnection);

            //Запускаем получение данных
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

            //Получаем коллекци
            Races = new ObservableCollection<BaseResponseListItem>(racesTask.Result.Items!);
            Months = new ObservableCollection<BaseResponseListItem>(monthsTask.Result.Items!);
            Countries = new ObservableCollection<BaseResponseListItem>(countriesTask.Result.Items!);
            TypesBodies = new ObservableCollection<BaseResponseListItem>(typesBodiesTask.Result.Items!);
            TypesFaces = new ObservableCollection<BaseResponseListItem>(typesFacesTask.Result.Items!);
            HairsColors = new ObservableCollection<BaseResponseListItem>(hairsColorsTask.Result.Items!);
            EyesColors = new ObservableCollection<BaseResponseListItem>(eyesColorsTask.Result.Items!);

            //Привязываем данные
            RacePicker.ItemsSource = Races;
            MonthBirtPicker.ItemsSource = Months;
            CountryPicker.ItemsSource = Countries;
            TypeBodyPicker.ItemsSource = TypesBodies;
            TypeFacePicker.ItemsSource = TypesFaces;
            HairsColorPicker.ItemsSource = HairsColors;
            EyesColorPicker.ItemsSource = EyesColors;

            //Добавляем биографию
            AddBiography();
        }
        catch (InnerException ex)
        {
            //Устанавливаем текст ошибки
            ErrorLabel.Text = ex.Message;
        }
        catch (Exception ex)
        {
            //Устанавливаем текст ошибки
            ErrorLabel.Text = ex.Message;
        }
        finally
        {
            //Останавливаем колесо загрузки
            LoadActivityIndicator.IsVisible = false;
            LoadActivityIndicator.IsRunning = false;

            //Возвращаем видимость элементов
            RegistrationHeroStackLayout.IsVisible = true;
        }
    }

    /// <summary>
    /// Событие нажатия на кнопку возвращения на предыдущую страницу
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void Back_Clicked(object sender, EventArgs e)
    {
        ToBack(null, null);
    }

    /// <summary>
    /// Событие изменения пола
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void Gender_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //Если значение выбрано, ставим пол - мужской
        if (GenderCheckBox.IsChecked) GenderLabel.Text = "Мужской";
        //Иначе - женский
        else GenderLabel.Text = "Женский";
    }

    /// <summary>
    /// Событие выбора расы
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void Race_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Если выбранный элемент пустой, выходим
        if (RacePicker.SelectedItem ==  null) return;

        //Запускаем колесо загрузки
        NationLoadActivityIndicator.IsRunning = true;
        NationStackLayout.IsVisible = false;

        //Обнуляем текст ошибки
        ErrorLabel.Text = null;

        try
        {
            //Проверяем наличие сервиса работы с нациями
            if (_nations == null) throw new InnerException(Errors.EmptyServiceNations);

            //Получаем выбранную расу
            long? raceId = ((BaseResponseListItem?) RacePicker.SelectedItem)?.Id;

            //Получаем коллекцию наций
            Nations = new ObservableCollection<BaseResponseListItem>((await _nations.GetNationsList(raceId)).Items!);

            //Привязываем данные
            NationPicker.ItemsSource = Nations;

            //Делаем доступным выпадающий список наций
            NationStackLayout.IsVisible = true;
        }
        catch (InnerException ex)
        {
            //Устанавливаем текст ошибки
            ErrorLabel.Text = ex.Message;
        }
        catch (Exception ex)
        {
            //Устанавливаем текст ошибки
            ErrorLabel.Text = ex.Message;
        }
        finally
        {
            //Останавливаем колесо загрузки
            NationLoadActivityIndicator.IsRunning = false;
        }
    }

    /// <summary>
    /// Событие выбора страны
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void Country_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Если выбранный элемент пустой, выходим
        if (CountryPicker.SelectedItem == null) return;

        //Запускаем колесо загрузки
        RegionLoadActivityIndicator.IsRunning = true;
        RegionStackLayout.IsVisible = false;
        AreaStackLayout.IsVisible = false;

        //Обнуляем текст ошибки
        ErrorLabel.Text = null;

        try
        {
            //Проверяем наличие сервиса работы с регионами
            if (_regions == null) throw new InnerException(Errors.EmptyServiceRegions);

            //Получаем выбранную страну
            long? countryId = ((BaseResponseListItem?)CountryPicker.SelectedItem)?.Id;

            //Получаем коллекцию регионов
            Regions = new ObservableCollection<BaseResponseListItem>((await _regions.GetRegionsList(countryId)).Items!);

            //Привязываем данные
            RegionPicker.ItemsSource = Regions;

            //Делаем доступным выпадающий список регионов
            RegionStackLayout.IsVisible = true;
        }
        catch (InnerException ex)
        {
            //Устанавливаем текст ошибки
            ErrorLabel.Text = ex.Message;
        }
        catch (Exception ex)
        {
            //Устанавливаем текст ошибки
            ErrorLabel.Text = ex.Message;
        }
        finally
        {
            //Останавливаем колесо загрузки
            RegionLoadActivityIndicator.IsRunning = false;
        }
    }

    /// <summary>
    /// Событие выбора региона
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void Region_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Если выбранный элемент пустой, выходим
        if (RegionPicker.SelectedItem == null) return;

        //Запускаем колесо загрузки
        AreaLoadActivityIndicator.IsRunning = true;
        AreaStackLayout.IsVisible = false;

        //Обнуляем текст ошибки
        ErrorLabel.Text = null;

        try
        {
            //Проверяем наличие сервиса работы с областями
            if (_areas == null) throw new InnerException(Errors.EmptyServiceAreas);

            //Получаем выбранный регион
            long? regionId = ((BaseResponseListItem?)RegionPicker.SelectedItem)?.Id;

            //Получаем коллекцию областей
            Areas = new ObservableCollection<BaseResponseListItem>((await _areas.GetAreasList(regionId, null)).Items!);

            //Привязываем данные
            AreaPicker.ItemsSource = Areas;

            //Делаем доступным выпадающий список областей
            AreaStackLayout.IsVisible = true;
        }
        catch (InnerException ex)
        {
            //Устанавливаем текст ошибки
            ErrorLabel.Text = ex.Message;
        }
        catch (Exception ex)
        {
            //Устанавливаем текст ошибки
            ErrorLabel.Text = ex.Message;
        }
        finally
        {
            //Останавливаем колесо загрузки
            AreaLoadActivityIndicator.IsRunning = false;
        }
    }

    /// <summary>
    /// Событие нажатия на кнопку загрузки изображения
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void Download_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Формируем новое окно загрузки фотографий
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Выберите изображение",
                FileTypes = FilePickerFileType.Images
            });

            //Выходим, если не выбрали изображение
            if (result == null) return;

            //Считываем поток
            var stream = await result.OpenReadAsync();

            //Записываем поток в источник изображения
            HeroImage.Source = ImageSource.FromStream(() => stream);
        }
        catch (InnerException ex)
        {
            //Устанавливаем текст ошибки
            ErrorLabel.Text = ex.Message;
        }
        catch (Exception ex)
        {
            //Устанавливаем текст ошибки
            ErrorLabel.Text = ex.Message;
        }
    }

    /// <summary>
    /// Событие нажатия на кнопку добавления биографии
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void AddBiorgaphy_Clicked(object sender, EventArgs e)
    {
        //Добавляем биографию
        AddBiography();
    }

    /// <summary>
    /// Событие нажатия на кнопку сохранения
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void Save_Clicked(object sender, EventArgs e)
    {
        //Запускаем колесо загрузки
        LoadActivityIndicator.IsRunning = true;
        RegistrationHeroStackLayout.IsVisible = false;

        try
        {
            //Обнуляем текст ошибки
            ErrorLabel.Text = null;

            //Регистрируем игрока
            //var user = await _users!.AddUser(_addUserRequest);

            //Регистрируем персонажа


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
            //Устанавливаем текст ошибки
            ErrorLabel.Text = ex.Message;
        }
        catch (Exception ex)
        {
            //Устанавливаем текст ошибки
            ErrorLabel.Text = ex.Message;
        }
        finally
        {
            //Останавливаем колесо загрузки
            LoadActivityIndicator.IsRunning = false;

            //Возвращаем видимость элементов
            RegistrationHeroStackLayout.IsVisible = true;
        }
    }

    /// <summary>
    /// Метод возврата на предыдущую страницу
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private async void ToBack(object? sender, EventArgs? e)
    {
        //Переходим на старую страницу
        await Navigation.PopAsync();
    }

    /// <summary>
    /// Метод добавления компонентов биографии
    /// </summary>
    private void AddBiography()
    {
        //Получаем стили
        Application.Current!.Resources.TryGetValue("TitleSecondary", out var labelStyle);
        Application.Current!.Resources.TryGetValue("EntryPrimary", out var entryStyle);
        Application.Current!.Resources.TryGetValue("PrimaryText", out var boxViewColor);
        Application.Current!.Resources.TryGetValue("PickerPrimary", out var pickerStyle);


        //Создаём стек дня начала и добавляем в коллекцию биографий
        StackLayout dayBegin = new() 
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(dayBegin);

        //Создаём компоненты дня начала и добавляем в коллекцию
        Label dayBiographyBeginLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "День начала*:",
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
            Placeholder = "Введите день начала",
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


        //Создаём стек месяца начала и добавляем в коллекцию биографий
        StackLayout monthBegin = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(monthBegin);

        //Создаём компоненты месяца начала и добавляем в коллекцию
        Label monthBiographyBeginLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Месяц начала*:",
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
            Title = "Выберите месяц начала",
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


        //Создаём стек цикла начала и добавляем в коллекцию биографий
        StackLayout cycleBegin = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(cycleBegin);

        //Создаём компоненты цикла начала и добавляем в коллекцию
        Label cycleBiographyBeginLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Цикл начала*:",
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
            Placeholder = "Введите цикл начала",
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


        //Создаём стек дня окончания и добавляем в коллекцию биографий
        StackLayout dayEnd = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(dayEnd);

        //Создаём компоненты дня окончания и добавляем в коллекцию
        Label dayBiographyEndLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "День окончания",
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
            Placeholder = "Введите день окончания",
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


        //Создаём стек месяца окончания и добавляем в коллекцию биографий
        StackLayout monthEnd = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(monthEnd);

        //Создаём компоненты месяца окончания и добавляем в коллекцию
        Label monthBiographyEndLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Месяц окончания*:",
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
            Title = "Выберите месяц окончания",
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


        //Создаём стек цикла окончания и добавляем в коллекцию биографий
        StackLayout cycleEnd = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(cycleEnd);

        //Создаём компоненты цикла окончания и добавляем в коллекцию
        Label cycleBiographyEndLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Цикл окончания*:",
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
            Placeholder = "Введите цикл окончания",
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


        //Создаём стек текста и добавляем в коллекцию биографий
        StackLayout text = new()
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        BiographyStackLayout.Add(text);
        
        //Создаём текст и добавляем в коллекцию
        Label textBiographyLabel = new()
        {
            HorizontalOptions = LayoutOptions.Start,
            Text = "Текст биографии*:",
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
            Placeholder = "Введите текст биографии",
            AutoSize = EditorAutoSizeOption.TextChanges,
            Style = (Style)entryStyle
        };
        textBiographyScroolView.Content = textBiographyEditor;

        //Создаём новый экземляр класса элемента биографии и добавляем его в коллекцию
        BiographyElement biographyElement = new(dayBiographyBeginEntry, monthBiographyBeginPicker, cycleBiographyBeginEntry,
            dayBiographyEndEntry, monthBiographyEndPicker, cycleBiographyEndEntry, textBiographyEditor);
        BiographyElements.Add(biographyElement);
    }

    /// <summary>
    /// Класс элемента биографии
    /// </summary>
    private class BiographyElement(Entry dayBegin, Picker monthBegin, Entry cycleBegin, Entry dayEnd, Picker monthEnd,
        Entry cycleEnd, Editor text)
    {
        /// <summary>
        /// Поле ввода даты начала
        /// </summary>
        public Entry? DayBegin { get; set; } = dayBegin;

        /// <summary>
        /// Выпадающий список месяцев начала
        /// </summary>
        public Picker? MonthBegin { get; set; } = monthBegin;

        /// <summary>
        /// Поле ввода цикла начала
        /// </summary>
        public Entry? CycleBegin { get; set; } = cycleBegin;

        /// <summary>
        /// Поле ввода даты окончания
        /// </summary>
        public Entry? DayEnd { get; set; } = dayEnd;

        /// <summary>
        /// Выпадающий список месяцев окончания
        /// </summary>
        public Picker? MonthEnd { get; set; } = monthEnd;

        /// <summary>
        /// Поле ввода даты окончания
        /// </summary>
        public Entry? CycleEnd { get; set; } = cycleEnd;

        /// <summary>
        /// Поле ввода текст биографии
        /// </summary>
        public Editor? Text { get; set; } = text;
    }
}