using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Schuldenbuch.Core.Database;
using Schuldenbuch.Core.Interfaces;
using Schuldenbuch.Core.Services;

namespace Schuldenbuch.Desktop;

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
			});

		builder.Services.AddMauiBlazorWebView();

        builder.Services.AddMauiBlazorWebView();

        // Hier wird der DbContext direkt mit deinem exakten Pfad registriert
        builder.Services.AddDbContext<SchuldenbuchContext>(options =>
            options.UseSqlite(@"Data Source=D:\ProgramData\Programmieren\Schuldenbuch\Database\schuldenbuch.db"));

        builder.Services.AddSingleton<IDatabase, SqliteDatabase>(); builder.Services.AddSingleton<IDatabase, SqliteDatabase>();
        builder.Services.AddSingleton<PersonService>();
        builder.Services.AddSingleton<DebtService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		//builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
