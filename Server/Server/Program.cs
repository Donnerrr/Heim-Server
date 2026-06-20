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

builder.Services.AddControllers(); // Füge die Controller-Services hinzu
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.WebHost.UseUrls("http://localhost:5002");
//builder.WebHost.UseUrls("http://0.0.0.0:5000");

// Hier zwingen wir den Server, auf allen IPs zu lauschen:
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

//var connectionString = $"Data Source={dbPath}";

// 2. Den DbContext mit dem Pfad füttern
builder.Services.AddDbContext<Server.Database.SchuldenbuchContext>(options =>
    options.UseNpgsql(connectionString), ServiceLifetime.Transient);


builder.Services.AddScoped<Schuldenbuch.Core.Interfaces.ISchuldenbuchDatabase, Server.Database.SchuldenbuchRepository>();
// Damit weiß das Lager, was 'IDatabase' ist und dass es die 'SqliteDatabase' nutzen soll!


builder.Services.AddScoped<IPersonService, PersonService>();

builder.Services.AddScoped<DebtService>();


builder.Host.UseWindowsService();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found");
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<Server.Components.App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(Schuldenbuch.Components.Dashboard).Assembly);

// Swagger nur in Entwicklung
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapControllers();

}


app.Run();
