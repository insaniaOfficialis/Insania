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
//builder.Services.AddAutoMapper(typeof(AppMappingProfile));

//��������� ��������� �������������
builder.Services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationContext>();
builder.Services.AddControllersWithViews();

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
});

//�������� ����������� ��� ��������

//������ ����������
var app = builder.Build();

//��������� ��������� �������� ��������
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

//�������� ����������
app.Run();