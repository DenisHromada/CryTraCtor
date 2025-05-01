using CryTraCtor.Common.Installers;
using CryTraCtor.Packet.Mappers;
using CryTraCtor.Packet.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CryTraCtor.Packet.Installers;

public class WebAppPacketInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddTransient<IDnsTrafficMapper, DnsTrafficMapper>()
            .AddTransient<DnsPacketReader>()
            .AddTransient<BitcoinMessageParser>()
            .AddTransient<BitcoinPacketAnalyzer>()
            .AddTransient<BitcoinTcpStreamHandler>()
            .AddTransient<IEndpointReader, EndpointReader>();
    }
}
