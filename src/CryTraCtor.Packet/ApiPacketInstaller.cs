using CryTraCtor.Common.Installers;
using CryTraCtor.Packet.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CryTraCtor.Packet;

public class ApiPacketInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddTransient<IDnsTransactionExtractor, DnsTransactionExtractor>();
    }
}