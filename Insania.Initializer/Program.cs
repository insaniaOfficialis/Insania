using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

using Insania.Entities.Context;
using Insania.Initializer.Initialization.InitializationDataBase;
using Insania.Models.Exceptions;
using Insania.Database.Entities.AccessRights;
using Insania.Database.Entities.Users;
using Insania.Models.Logging;

namespace Insania.Initializer;

/// <summary>
/// Основной класс приложения
/// </summary>
public class Program
{
    /// <summary>
    /// Коллекция сервисов
    /// </summary>
    public static readonly ServiceProvider _serviceProvider;

    /// <summary>
    /// Статический конструктор основного класса приложения
    /// </summary>
    static Program()
    {
        //Создаём коллекцию сервисов
        ServiceCollection services = new();

        //Добавляем файл конфигурации
        IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();

        //Добавляем параметры для контекста базы данных
        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultPostgresConnectionString"));
            options.EnableSensitiveDataLogging();
        });

        //Добавляем параметры идентификации
        services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationContext>();

        //Добавляем параметры логирования
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.File(path: configuration["LoggingOptions:FilePath"]!, rollingInterval: RollingInterval.Day)
            .WriteTo.Debug()
            .CreateLogger();
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

        //Добавляем зависимости сервиса
        services.AddSingleton(configuration); //сервис конфигурации
        services.AddScoped<IInitializationDataBase, InitializationDataBase>(); //сервис инициализации базы данных

        //Строим сервисы
        _serviceProvider = services.BuildServiceProvider();
    }

    /// <summary>
    /// Основной метод приложения
    /// </summary>
    public static void Main()
    {
        //Получаем сервис инициализации базы данных
        IInitializationDataBase initializationDataBase = _serviceProvider .GetRequiredService<IInitializationDataBase>() 
            ?? throw new InnerException(Errors.EmptyServiceInitializtionDataBase);

        //Запускаем инициализацию базы данных
        initializationDataBase.Initialization().Wait();
    }
}