using System.Text.Encodings.Web;
using System.Text.Unicode;

using Microsoft.Extensions.WebEncoders;

using Serilog;

//������ ��������� ����������� ����������
var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

//��������� ��������� ������� razor
builder.Services.AddRazorPages();

//��������� ��������� ��������
builder.Services.AddMvc().AddRazorPagesOptions(options =>
{
    options.Conventions.AddPageRoute("/Users/Authentication/Index", "");
});

//��������� ������� �����
builder.Services.Configure<WebEncoderOptions>(options =>
{
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
});

//��������� ��������� �����������
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.File(path: config["LoggingOptions:FilePath"]!, rollingInterval: RollingInterval.Day)
    .WriteTo.Debug()
    .CreateLogger();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

//������ ����������
var app = builder.Build();

//��������� ��������������� � http �� https
app.UseHttpsRedirection();

//��������� ����������� �����
app.UseStaticFiles();

//��������� ��������
app.UseRouting();

//��������� �����������
app.UseAuthorization();

//��������� ��������� ������������� ������������
app.MapControllerRoute(name: "default", pattern: "{controller=AuthenticationController}/{action=view}");

//��������� ��������� �������������
app.MapRazorPages();

//��������� ����������
app.Run();
