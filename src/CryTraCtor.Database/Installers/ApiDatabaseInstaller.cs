using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Factories;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.Repositories;
using CryTraCtor.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CryTraCtor.Database.Installers;

public class ApiDatabaseInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IFileStorageService, LocalFileStorageService>()
            .AddSingleton<IDbContextFactory<CryTraCtorDbContext>, DbContextPgsqlFactory>()
            .AddSingleton<IStoredFileRepository, StoredFileRepository>()
            .AddSingleton<IEntityMapper<StoredFileEntity>, StoredFileMapper>();
        

        serviceCollection.AddScoped<StoredFileRepository>();
        // serviceCollection.Scan(selector =>
        //     selector.FromAssemblyOf<ApiDatabaseInstaller>()
        //         .AddClasses(classes => classes.AssignableTo<IStoredFileRepository>())
        //         .AsMatchingInterface()
        //         .WithScopedLifetime()
        // );
    }
}