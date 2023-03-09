using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using People.Models;

namespace People
{
public static class MauiProgram
{
public static MauiApp CreateMauiApp()
{
var builder = MauiApp.CreateBuilder();
builder
.UseMauiApp<App>()
.ConfigureFonts(fonts =>
{
fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
})
.ConfigureServices((services) =>
{
string dbPath = FileAccessHelper.GetLocalFilePath("people.db3");
services.AddSingleton<PersonRepository>(s => new PersonRepository(dbPath));
});

        return builder.Build();
    }
}
}