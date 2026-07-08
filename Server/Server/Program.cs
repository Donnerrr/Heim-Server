using Microsoft.EntityFrameworkCore;
using Schuldenbuch.Core.Services;
using Schuldenbuch.Core.Interfaces;


var baseDir = AppDomain.CurrentDomain.BaseDirectory;
var dataFolder = Path.Combine(baseDir, "Data");

// 1. Prüfen, ob der Ordner da ist, wenn nicht: Anlegen
if (!Directory.Exists(dataFolder))
{
    try
    {
        Directory.CreateDirectory(dataFolder);
        Console.WriteLine("Ordner erfolgreich erstellt unter: " + dataFolder);
    }
    catch (Exception ex)
    {
        Console.WriteLine("KRITISCH: Konnte Ordner nicht erstellen! Fehler: " + ex.Message);
        // Hier siehst du sofort, ob es an Rechten liegt (z.B. UnauthorizedAccessException)
    }
}


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPWA", policy =>
    {
        policy.WithOrigins(
            "https://finanzen.pottanker.de", // Nur das ist wirklich nötig
            "https://www.finanzen.pottanker.de"
        )
        .AllowAnyMethod()
        .AllowAnyHeader();
        // .AllowCredentials() ist optional
    });
});

builder.Services.AddControllers(); // Füge die Controller-Services hinzu
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5149); // HTTP
  //
  //options.ListenAnyIP(7033, listenOptions =>
  //  {
  //      listenOptions.UseHttps(); // HTTPS
  //  });
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Der ContentRootPath ist der Ordner, von dem aus die Anwendung läuft. Das ist wichtig, damit wir wissen, wo die appsettings.json liegt.
string contenRoot = builder.Environment.ContentRootPath;

// Hier bauen wir den Pfad zur Datenbank zusammen. Das ist wichtig, damit die Anwendung weiß, wo sie die Datenbank findet. In diesem Fall liegt sie im "Data"-Ordner, der im ContentRoot liegt.
string dbFolder = Path.Combine(contenRoot, "Data");
string dbPath = Path.Combine(dbFolder, "heimserver.db");

if(!Directory.Exists(dbFolder))
{
    Directory.CreateDirectory(dbFolder);
}



// 1. Connection String aus der appsettings.json laden
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString.Contains("{ContentRoot}"))
{
    connectionString = connectionString.Replace("{ContentRoot}", contenRoot);
}

// 2. Den DbContext mit dem Pfad füttern
builder.Services.AddDbContext<Server.Database.SchuldenbuchContext>(options =>
    //options.UseNpgsql(connectionString), ServiceLifetime.Transient);
    options.UseSqlite($"Data Source={dbPath}"), ServiceLifetime.Transient);

builder.Services.AddScoped<Schuldenbuch.Core.Interfaces.ISchuldenbuchDatabase, Server.Database.SchuldenbuchRepository>(); // DI für die Datenbank-Repository-Klasse




builder.Services.AddScoped<IPersonService, PersonService>();

builder.Services.AddScoped<IDebtService, DebtService>();


var app = builder.Build();

// 1. Basis-Sicherheits- & Fehler-Handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found");
//app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

// 2. Swagger (Nur im Dev-Modus)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 3. CORS (MUSS zwingend vor dem Routing und den Controllern kommen!)
app.UseCors("AllowPWA");

// 4. Blazor & Komponenten-Routing
app.MapRazorComponents<Server.Components.App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(Schuldenbuch.Components.Dashboard).Assembly);

// 5. API-Controller (Aus der IF-Bedingung befreit, damit sie immer greifen)
app.MapControllers();

app.Run();
