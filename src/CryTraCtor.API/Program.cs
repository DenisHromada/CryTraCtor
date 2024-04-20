using CryTraCtor.Business.Installers;
using CryTraCtor.Database.Extensions;
using CryTraCtor.Database.Installers;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers()
    ;

builder.Services.AddInstaller<ApiDatabaseInstaller>();
builder.Services.AddInstaller<ApiBusinessInstaller>();

var app = builder.Build();

var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option);

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();