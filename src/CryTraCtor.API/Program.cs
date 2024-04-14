using CryTraCtor.Database;
using CryTraCtor.Database.Factories;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers()
    ;

builder.Services.AddSingleton<IDbContextFactory<CryTraCtorDbContext>, DbContextPgsqlFactory>();

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