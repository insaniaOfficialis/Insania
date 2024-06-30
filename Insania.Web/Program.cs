using System.Text.Encodings.Web;
using System.Text.Unicode;

using Microsoft.Extensions.WebEncoders;

using Serilog;

//Создаём экземпляр построителя приложения
var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

//Добавляем поддержку страниц razor
builder.Services.AddRazorPages();

//Добавляем стартовую страницу
builder.Services.AddMvc().AddRazorPagesOptions(options =>
{
    options.Conventions.AddPageRoute("/Users/Authentication/Index", "");
});

//Добавляем русский текст
builder.Services.Configure<WebEncoderOptions>(options =>
{
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
});

//Добавляем параметры логирования
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.File(path: config["LoggingOptions:FilePath"]!, rollingInterval: RollingInterval.Day)
    .WriteTo.Debug()
    .CreateLogger();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

//Строим приложение
var app = builder.Build();

//Добавляем перенаправление с http на https
app.UseHttpsRedirection();

//Добавляем статические файлы
app.UseStaticFiles();

//Добавляем переходы
app.UseRouting();

//Добавляем авторизацию
app.UseAuthorization();

//Добавляем поддержку маршрутизации контроллеров
app.MapControllerRoute(name: "default", pattern: "{controller=AuthenticationController}/{action=view}");

//Добавляем поддержку маршрутизации
app.MapRazorPages();

//Запускаем приложение
app.Run();
