using CryTraCtor.Business.Installers;
using CryTraCtor.Common.Extensions;
using CryTraCtor.Database.Installers;
using CryTraCtor.Packet.Installers;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers()
    ;

builder.Services.AddInstaller<ApiPacketInstaller>();
builder.Services.AddInstaller<ApiDatabaseInstaller>();
builder.Services.AddInstaller<ApiBusinessInstaller>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option);

// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
