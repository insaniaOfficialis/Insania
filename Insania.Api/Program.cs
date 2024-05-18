using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

using Insania.Entities.Context;
using Insania.Database.Entities.AccessRights;
using Insania.Database.Entities.Users;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

//¬водим переменные дл€ токена
var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["TokenOptions:Key"]!));
var issuer = config["TokenOptions:Issuer"];
var audience = config["TokenOptions:Audience"];

//ƒобавл€ем параметры дл€ контекста базы данных
services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(config.GetConnectionString("DefaultPostgresConnectionString"));
    options.EnableSensitiveDataLogging();
});

//ƒобавл€ем параметры маппера моделей
//builder.Services.AddAutoMapper(typeof(AppMappingProfile));

//ƒобавл€ем параметры идентификации
builder.Services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationContext>();
builder.Services.AddControllersWithViews();

//ƒобавл€ем параметры авторизации
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Bearer", new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes("Bearer")
    .RequireAuthenticatedUser().Build());

//ƒобавл€ем параметры сериализации и десериализации json
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.WriteIndented = true;
});

//ƒобавл€ем параметры политики паролей
services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});

//ƒобавл€ем параметры аутентификации
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироватьс€ издатель при валидации токена
            ValidateIssuer = true,
            // строка, представл€юща€ издател€
            ValidIssuer = issuer,
            // будет ли валидироватьс€ потребитель токена
            ValidateAudience = true,
            // установка потребител€ токена
            ValidAudience = audience,
            // будет ли валидироватьс€ врем€ существовани€
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = key,
            // валидаци€ ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });

//ƒобавл€ем параметры логировани€
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.File(path: config["LoggingOptions:FilePath"]!, rollingInterval: RollingInterval.Day)
    .WriteTo.Debug()
    .CreateLogger();
services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

//ƒобавл€ем параметры документации
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Insania API", Version = "v1" });

    var filePath = Path.Combine(AppContext.BaseDirectory, "Insania.Api.xml");
    options.IncludeXmlComments(filePath);
});

//¬недр€ем зависимости дл€ сервисов

//—троим приложение
var app = builder.Build();

//ƒобавл€ем параметры конвеера запросов
//app.UseMiddleware<LoggingMiddleware>();

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

//«апускае приложение
app.Run();