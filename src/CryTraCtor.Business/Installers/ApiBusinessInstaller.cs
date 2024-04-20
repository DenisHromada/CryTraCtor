using CryTraCtor.Business.Facades;
using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers;
using CryTraCtor.Business.Mappers.CryptoProduct;
using CryTraCtor.Business.Mappers.KnownDomain;
using CryTraCtor.Database.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace CryTraCtor.Business.Installers;

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