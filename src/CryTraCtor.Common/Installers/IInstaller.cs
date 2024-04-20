using Microsoft.Extensions.DependencyInjection;

namespace CryTraCtor.Common.Installers;

public interface IInstaller
{
    void Install(IServiceCollection serviceCollection);
}