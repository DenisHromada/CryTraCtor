using Microsoft.Extensions.DependencyInjection;

namespace CryTraCtor.Database.Installers;

public interface IInstaller
{
    void Install(IServiceCollection serviceCollection);
}