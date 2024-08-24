using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

using Insania.Api.Middleware;
using Insania.Api.Swagger;
using Insania.BusinessLogic.Administrators.Administrators;
using Insania.BusinessLogic.Appearance.EyesColors;
using Insania.BusinessLogic.Appearance.HairsColors;
using Insania.BusinessLogic.Appearance.TypesBodies;
using Insania.BusinessLogic.Appearance.TypesFaces;
using Insania.BusinessLogic.Biology.Nations;
using Insania.BusinessLogic.Biology.Races;
using Insania.BusinessLogic.Chronology.Months;
using Insania.BusinessLogic.Files.Files;
using Insania.BusinessLogic.Heroes.BiographiesHeroes;
using Insania.BusinessLogic.Heroes.BiographiesRequestsHeroesRegistration;
using Insania.BusinessLogic.Heroes.Heroes;
using Insania.BusinessLogic.Heroes.RequestsHeroesRegistration;
using Insania.BusinessLogic.Heroes.StatusesRequestsHeroesRegistration;
using Insania.BusinessLogic.OutOfCategories;
using Insania.BusinessLogic.Politics.Areas;
using Insania.BusinessLogic.Politics.Countries;
using Insania.BusinessLogic.Politics.Regions;
using Insania.BusinessLogic.Sociology.PrefixesNames;
using Insania.BusinessLogic.System.Parameters;
using Insania.BusinessLogic.Users.Authentication;
using Insania.BusinessLogic.Users.Users;
using Insania.Database.Entities.AccessRights;
using Insania.Database.Entities.Users;
using Insania.Entities.Context;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

//Введение переменных для токена
var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["TokenOptions:Key"]!));
var issuer = config["TokenOptions:Issuer"];
var audience = config["TokenOptions:Audience"];

//Добавление параметров для контекста базы данных
services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(config.GetConnectionString("DefaultPostgresConnectionString"));
    options.EnableSensitiveDataLogging();
});

//Добавление параметров преобразования моделей
builder.Services.AddAutoMapper(typeof(AppMappingProfile));

//Добавление параметров идентификации
builder.Services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationContext>();
builder.Services.AddControllersWithViews();

//Установка игнорирования типов даты и времени
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//Добавление параметров авторизации
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Bearer", new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes("Bearer")
    .RequireAuthenticatedUser().Build());

//Добавление параметров сериализации и десериализации json
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.WriteIndented = true;
});

//Добавление параметров политики паролей
services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});

//Добавление параметров аутентификации
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = issuer,
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = audience,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = key,
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });

//Добавление параметров логирования
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.File(path: config["LoggingOptions:FilePath"]!, rollingInterval: RollingInterval.Day)
    .WriteTo.Debug()
    .CreateLogger();
services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

//Добавление параметров документации
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Insania API", Version = "v1" });

    var filePath = Path.Combine(AppContext.BaseDirectory, "Insania.Api.xml");
    options.IncludeXmlComments(filePath);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Авторизация по ключу приложения",
        Scheme = "Bearer"
    });
    options.OperationFilter<AuthenticationRequirementsOperationFilter>();
});

//Добавление корсов
builder.Services.AddCors(options =>
{
    options.AddPolicy("BadPolicy", policyBuilder => policyBuilder
        .SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
    );

    options.DefaultPolicyName = "BadPolicy";
});

//Внедрение зависимостей для сервисов
services.AddScoped<IAuthentication, AuthenticationService>(); //аутентифкация
services.AddScoped<IUsers, UsersService>(); //сервис работы с пользователями
services.AddScoped<IRaces, RacesService>(); //сервис работы с расами
services.AddScoped<INations, NationsService>(); //сервис работы с нациями
services.AddScoped<IMonths, MonthsService>(); //сервис работы с месяцами
services.AddScoped<ICountries, CountriesService>(); //сервис работы со странами
services.AddScoped<IRegions, RegionsService>(); //сервис работы с регионами
services.AddScoped<IAreas, AreasService>(); //сервис работы с областями
services.AddScoped<ITypesBodies, TypesBodiesService>(); //сервис работы с типами телосложений
services.AddScoped<ITypesFaces, TypesFacesService>(); //сервис работы с типами лиц
services.AddScoped<IHairsColors, HairsColorsService>(); //сервис работы с цветами волос
services.AddScoped<IEyesColors, EyesColorsService>(); //сервис работы с цветами глаз
services.AddScoped<IPrefixesNames, PrefixesNamesService>(); //сервис работы с префиксами имён
services.AddScoped<IParameters, ParametersService>(); //сервис работы с префиксами имён
services.AddScoped<IPrefixesNames, PrefixesNamesService>(); //сервис работы с параметрами
services.AddScoped<IHeroes, HeroesService>(); //сервис работы с персонажами
services.AddScoped<IFiles, FilesService>(); //сервис работы с файлами
services.AddScoped<IRequestsHeroesRegistration, ReuestsHeroRegistrationService>(); //сервис работы с заявками на регистрацию персонажей
services.AddScoped<IStatusesRequestsHeroesRegistration, StatusesRequestsHeroesRegistrationService>(); //сервис работы со статусами заявок на регистрацию персонажей
services.AddScoped<IAdministrators, AdministratorsService>(); //сервис работы с администраторами
services.AddScoped<IBiographiesHeroes, BiographiesHeroesService>(); //сервис работы с биографиями персонажей
services.AddScoped<IBiographiesRequestsHeroesRegistration, BiographiesRequestsHeroesRegistrationService>(); //сервис работы с биографиями заявок на регистрацию персонажей

//Построение приложения
var app = builder.Build();

//Добавление параметров конвеера запросов
app.UseMiddleware<LoggingMiddleware>();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=RegistrationController}/{action=Check}");
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Insania API V1");
});
app.UseCors();

app.MapGet("/", () => "Hello World!");

//Запуск приложения
app.Run();