namespace Insania.App;

/// <summary>
/// Класс приложения
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Ширина
    /// </summary>
    private const int Width = 1200;

    /// <summary>
    /// Высота
    /// </summary>
    private const int Height = 750;

    /// <summary>
    /// Коллекция сервисов
    /// </summary>
    public static IServiceProvider? Services { get; set; }

    /// <summary>
    /// Конструктор класс приложения
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    public App(IServiceProvider services)
    {
        try
        {
            //Инициализируем компоненты
            InitializeComponent();

            //Получаем коллекцию сервисов
            Services = services;

            //Устанавливаем основную страницу
            MainPage = new NavigationPage(new MainPage());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// Событие создания окна
    /// </summary>
    /// <param name="activationState">Изменения состояния</param>
    /// <returns></returns>
    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        window.MinimumHeight = Height;
        window.Height = Height;
        window.MinimumWidth = Width;
        window.Width = Width;
        return window;
    }
}