using CryTraCtor.Common.Installers;
using CryTraCtor.Packet.DataTypeMappers;
using CryTraCtor.Packet.Mappers;
using CryTraCtor.Packet.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CryTraCtor.Packet;

public class ApiPacketInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddTransient<IDnsTrafficMapper, DnsTrafficMapper>()
            .AddTransient<IDnsTransactionExtractor, DnsTransactionExtractor>()
            .AddTransient<IDnsPacketReader, DnsPacketReader>();
    }
}