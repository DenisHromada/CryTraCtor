using CryTraCtor.Business.Facades;
using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers;
using CryTraCtor.Business.Mappers.Bitcoin;
using CryTraCtor.Business.Mappers.CryptoProduct;
using CryTraCtor.Business.Mappers.FileAnalysis;
using CryTraCtor.Business.Mappers.KnownDomain;
using CryTraCtor.Business.Mappers.TrafficParticipant;
using CryTraCtor.Business.Mappers.DomainMatch;
using CryTraCtor.Business.Services;
using CryTraCtor.Common.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace CryTraCtor.Business.Installers;

public class WebAppBusinessInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<StoredFileModelMapper>()
            .AddSingleton<CryptoProductListModelMapper>()
            .AddSingleton<CryptoProductModelMapper>()
            .AddSingleton<KnownDomainListModelMapper>()
            .AddSingleton<KnownDomainModelMapper>()
            .AddSingleton<KnownDomainImportModelMapper>()
            .AddSingleton<TrafficParticipantListModelMapper>()
            .AddSingleton<TrafficParticipantModelMapper>()
            .AddSingleton<FileAnalysisListModelMapper>()
            .AddSingleton<FileAnalysisModelMapper>()
            .AddSingleton<DnsMessageModelMapper>()
            .AddSingleton<DomainMatchMapper>()
            .AddSingleton<BitcoinPacketModelMapper>()
            .AddSingleton<BitcoinTransactionMapper>()
            .AddSingleton<BitcoinBlockHeaderMapper>();

        serviceCollection
            .AddScoped<IStoredFileFacade, StoredFileFacade>()
            .AddScoped<ICryptoProductFacade, CryptoProductFacade>()
            .AddScoped<IKnownDomainFacade, KnownDomainFacade>()
            .AddScoped<IKnownDomainImportFacade, KnownDomainImportFacade>()
            .AddScoped<ITrafficParticipantFacade, TrafficParticipantFacade>()
            .AddScoped<IFileAnalysisFacade, FileAnalysisFacade>()
            .AddScoped<IDnsMessageFacade, DnsMessageFacade>()
            .AddScoped<IDomainMatchFacade, DomainMatchFacade>()
            .AddScoped<IBitcoinMessageFacade, BitcoinMessageFacade>()
            .AddScoped<IBitcoinInventoryFacade, BitcoinInventoryFacade>();

        serviceCollection
            .AddScoped<FileAnalysisService>()
            .AddScoped<EndpointAnalysisService>()
            .AddScoped<DnsAnalysisService>()
            .AddScoped<DomainMatchAssociationService>()
            .AddScoped<BitcoinAnalysisService>()
            .AddScoped<BitcoinEndpointAssociationService>()
            .AddScoped<CsvService>()

            ;
    }
}
