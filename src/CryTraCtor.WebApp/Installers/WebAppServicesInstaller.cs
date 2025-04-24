using CryTraCtor.Common.Installers;
using CryTraCtor.WebApp.Services;

namespace CryTraCtor.WebApp.Installers;

public class WebAppServicesInstaller : IInstaller
{
    public void Install(IServiceCollection services)
    {
        services.AddScoped<IBreadcrumbService, BreadcrumbService>();
    }
}
