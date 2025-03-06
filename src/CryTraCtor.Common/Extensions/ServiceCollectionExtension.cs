using CryTraCtor.Common.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace CryTraCtor.Common.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInstaller<TInstaller>(this IServiceCollection services)
        where TInstaller : IInstaller, new()
    {
        var installer = new TInstaller();
        installer.Install(services);
    }
}
