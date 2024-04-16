using CryTraCtor.Database.Installers;
using CryTraCtor.Facades;
using CryTraCtor.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace CryTraCtor.Installers;

public class ApiBusinessInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<StoredFileModelMapper>();
        serviceCollection.AddScoped<IStoredFileFacade, StoredFileFacade>();
        // serviceCollection.Scan(selector =>
        //     selector.FromAssemblyOf<ApiBusinessInstaller>()
        //         .AddClasses(classes => classes.AssignableTo<IFacade>())
        //         .AsSelfWithInterfaces()
        //         .WithScopedLifetime()
        // );
    }
}