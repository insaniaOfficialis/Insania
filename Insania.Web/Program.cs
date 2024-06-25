//������ ��������� ����������� ����������
using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

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
