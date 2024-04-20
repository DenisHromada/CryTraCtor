using CryTraCtor.Common.Installers;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Factories;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.Repositories;
using CryTraCtor.Database.Services;
using CryTraCtor.Database.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CryTraCtor.Database.Installers;

public class ApiDatabaseInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IDbContextFactory<CryTraCtorDbContext>, DbContextPgsqlFactory>()
            .AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();

        serviceCollection
            .AddSingleton<IFileStorageService, LocalFileStorageService>();

        serviceCollection
            .AddSingleton<IEntityMapper<StoredFileEntity>, StoredFileEntityMapper>()
            .AddSingleton<IEntityMapper<CryptoProductEntity>, CryptoProductEntityMapper>()
            .AddSingleton<IEntityMapper<KnownDomainEntity>, KnownDomainEntityMapper>();

        serviceCollection
            .AddScoped<IStoredFileRepository, StoredFileRepository>();
    }
}