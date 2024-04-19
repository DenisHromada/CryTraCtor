using CryTraCtor.Database.Installers;
using CryTraCtor.Facades;
using CryTraCtor.Facades.Interfaces;
using CryTraCtor.Mappers;
using CryTraCtor.Mappers.CryptoProduct;
using CryTraCtor.Mappers.KnownDomain;
using Microsoft.Extensions.DependencyInjection;

namespace CryTraCtor.Installers;

public class ApiBusinessInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<StoredFileModelMapper>()
            .AddSingleton<CryptoProductListModelMapper>()
            .AddSingleton<CryptoProductModelMapper>()
            .AddSingleton<KnownDomainListModelMapper>()
            .AddSingleton<KnownDomainModelMapper>();
        
        serviceCollection
            .AddScoped<IStoredFileFacade, StoredFileFacade>()
            .AddScoped<ICryptoProductFacade, CryptoProductFacade>()
            .AddScoped<IKnownDomainFacade, KnownDomainFacade>();
    }
}