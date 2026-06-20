using Microsoft.EntityFrameworkCore;
using Schuldenbuch.BlazorWebApp.Components;
using Schuldenbuch.Core.Database;
using Schuldenbuch.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<SchuldenbuchContext>();

builder.Services.AddScoped<Schuldenbuch.Core.Interfaces.IDatabase, Schuldenbuch.Core.Database.SqliteDatabase>();

builder.Services.AddScoped<PersonService>();
// Damit weiß das Lager, was 'IDatabase' ist und dass es die 'SqliteDatabase' nutzen soll!

builder.Services.AddScoped<DebtService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(Schuldenbuch.Components.Dashboard).Assembly);

app.Run();
