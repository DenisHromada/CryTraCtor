using CryTraCtor.Business.Installers;
using CryTraCtor.Common.Extensions;
using CryTraCtor.Database.Installers;
using CryTraCtor.Packet.Installers;
using MudBlazor.Services;
using CryTraCtor.WebApp.Components;
using CryTraCtor.WebApp.Installers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddInstaller<WebAppPacketInstaller>();
builder.Services.AddInstaller<WebAppDatabaseInstaller>();
builder.Services.AddInstaller<WebAppBusinessInstaller>();
builder.Services.AddInstaller<WebAppOptionsInstaller>();

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
