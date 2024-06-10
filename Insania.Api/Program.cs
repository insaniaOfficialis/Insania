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
using Insania.BusinessLogic.Heroes.BiographiesHeroes;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

//������ ���������� ��� ������
var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["TokenOptions:Key"]!));
var issuer = config["TokenOptions:Issuer"];
var audience = config["TokenOptions:Audience"];

//��������� ��������� ��� ��������� ���� ������
services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(config.GetConnectionString("DefaultPostgresConnectionString"));
    options.EnableSensitiveDataLogging();
});

//��������� ��������� ������� �������
builder.Services.AddAutoMapper(typeof(AppMappingProfile));

//��������� ��������� �������������
builder.Services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationContext>();
builder.Services.AddControllersWithViews();

//������������� ������������� ����� ���� � �������
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//��������� ��������� �����������
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Bearer", new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes("Bearer")
    .RequireAuthenticatedUser().Build());

//��������� ��������� ������������ � �������������� json
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.WriteIndented = true;
});

//��������� ��������� �������� �������
services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});

//��������� ��������� ��������������
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // ���������, ����� �� �������������� �������� ��� ��������� ������
            ValidateIssuer = true,
            // ������, �������������� ��������
            ValidIssuer = issuer,
            // ����� �� �������������� ����������� ������
            ValidateAudience = true,
            // ��������� ����������� ������
            ValidAudience = audience,
            // ����� �� �������������� ����� �������������
            ValidateLifetime = true,
            // ��������� ����� ������������
            IssuerSigningKey = key,
            // ��������� ����� ������������
            ValidateIssuerSigningKey = true,
        };
    });

//��������� ��������� �����������
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.File(path: config["LoggingOptions:FilePath"]!, rollingInterval: RollingInterval.Day)
    .WriteTo.Debug()
    .CreateLogger();
services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

//��������� ��������� ������������
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
        Description = "����������� �� ����� ����������",
        Scheme = "Bearer"
    });
    options.OperationFilter<AuthenticationRequirementsOperationFilter>();
});

//�������� ����������� ��� ��������
services.AddScoped<IAuthentication, AuthenticationService>(); //�������������
services.AddScoped<IUsers, UsersService>(); //������ ������ � ��������������
services.AddScoped<IRaces, RacesService>(); //������ ������ � ������
services.AddScoped<INations, NationsService>(); //������ ������ � �������
services.AddScoped<IMonths, MonthsService>(); //������ ������ � ��������
services.AddScoped<ICountries, CountriesService>(); //������ ������ �� ��������
services.AddScoped<IRegions, RegionsService>(); //������ ������ � ���������
services.AddScoped<IAreas, AreasService>(); //������ ������ � ���������
services.AddScoped<ITypesBodies, TypesBodiesService>(); //������ ������ � ������ ������������
services.AddScoped<ITypesFaces, TypesFacesService>(); //������ ������ � ������ ���
services.AddScoped<IHairsColors, HairsColorsService>(); //������ ������ � ������� �����
services.AddScoped<IEyesColors, EyesColorsService>(); //������ ������ � ������� ����
services.AddScoped<IPrefixesNames, PrefixesNamesService>(); //������ ������ � ���������� ���
services.AddScoped<IParameters, ParametersService>(); //������ ������ � ���������� ���
services.AddScoped<IPrefixesNames, PrefixesNamesService>(); //������ ������ � �����������
services.AddScoped<IHeroes, HeroesService>(); //������ ������ � �����������
services.AddScoped<IFiles, FilesService>(); //������ ������ � �������
services.AddScoped<IRequestsHeroesRegistration, ReuestsHeroRegistrationService>(); //������ ������ � �������� �� ����������� ����������
services.AddScoped<IStatusesRequestsHeroesRegistration, StatusesRequestsHeroesRegistrationService>(); //������ ������ �� ��������� ������ �� ����������� ����������
services.AddScoped<IAdministrators, AdministratorsService>(); //������ ������ � ����������������
services.AddScoped<IBiographiesHeroes, BiographiesHeroesService>(); //������ ������ � ����������� ����������

//������ ����������
var app = builder.Build();

//��������� ��������� �������� ��������
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

app.MapGet("/", () => "Hello World!");

//��������� ����������
app.Run();