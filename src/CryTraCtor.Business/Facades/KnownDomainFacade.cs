using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers.KnownDomain;
using CryTraCtor.Business.Models.KnownDomain;
using CryTraCtor.Business.Models.Aggregates;
using CryTraCtor.Database.Entities;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Business.Facades;

public class KnownDomainFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    KnownDomainModelMapper modelMapper
) : FacadeBase<
        KnownDomainEntity,
        KnownDomainListModel,
        KnownDomainDetailModel,
        KnownDomainEntityMapper>(unitOfWorkFactory, modelMapper),
    IKnownDomainFacade
{
    protected override ICollection<string> IncludesNavigationPathDetail =>
        [$"{nameof(KnownDomainEntity.CryptoProduct)}"];

    public async Task<IEnumerable<KnownDomainDetailModel>> GetAllDetailAsync()
    {
        var unitOfWork = UnitOfWorkFactory.Create();
        var query = unitOfWork
            .GetRepository<KnownDomainEntity, KnownDomainEntityMapper>()
            .Get();

        foreach (var includePath in IncludesNavigationPathDetail)
        {
            query = query.Include($"{nameof(KnownDomainEntity.CryptoProduct)}");
        }

        var a = await query
            .ToListAsync()
            .ConfigureAwait(false);
        return modelMapper.MapToDetailModel(a);
    }

    public async Task<IEnumerable<KnownServiceDataModel>> GetKnownServicesDataAsync(Guid fileAnalysisId)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var knownDomainRepository = uow.GetRepository<KnownDomainEntity, KnownDomainEntityMapper>();
        var domainMatchRepository = uow.DomainMatches;
        var dnsMessageRepository = uow.GetRepository<DnsMessageEntity, DnsMessageEntityMapper>();

        var knownDomains = await knownDomainRepository.Get()
            .Include(kd => kd.CryptoProduct)
            .ToListAsync()
            .ConfigureAwait(false);

        var result = new List<KnownServiceDataModel>();

        foreach (var kd in knownDomains)
        {
            var domainMatches = await domainMatchRepository.GetByKnownDomainIdAsync(kd.Id)
                .ConfigureAwait(false);

            var relevantDnsMessageIds = domainMatches
                .Select(dm => dm.DnsMessageId)
                .Distinct()
                .ToList();

            if (!relevantDnsMessageIds.Any())
            {
                continue;
            }

            var allRelatedDnsMessages = await dnsMessageRepository.Get()
                .Where(dm => relevantDnsMessageIds.Contains(dm.Id) && dm.FileAnalysisId == fileAnalysisId)
                .Include(dm => dm.Sender)
                .Include(dm => dm.Recipient)
                .Include(dm => dm.ResolvedTrafficParticipants)
                .ThenInclude(dmrtp => dmrtp.TrafficParticipant)
                .ToListAsync()
                .ConfigureAwait(false);

            var queryDnsMessages = allRelatedDnsMessages.Where(dm => dm.IsQuery).ToList();
            var responseDnsMessages = allRelatedDnsMessages.Where(dm => !dm.IsQuery).ToList();

            var queryingEndpoints = queryDnsMessages
                .Select(dm => dm.Sender != null ? $"{dm.Sender.Address}:{dm.Sender.Port}" : null)
                .Where(endpointString => !string.IsNullOrEmpty(endpointString))
                .Distinct()
                .ToList();

            if (queryingEndpoints.Count == 0)
            {
                continue;
            }

            var resolvedIpAddresses = responseDnsMessages
                .SelectMany(dm =>
                    dm.ResolvedTrafficParticipants ?? new List<DnsMessageResolvedTrafficParticipantEntity>())
                .Select(dmrtp => dmrtp.TrafficParticipant != null ? dmrtp.TrafficParticipant.Address : null)
                .Where(ip => !string.IsNullOrEmpty(ip))
                .Distinct()
                .ToList();

            var communicatingEndpoints = new List<string>();

            if (resolvedIpAddresses.Count != 0)
            {
                var trafficParticipantRepository =
                    uow.GetRepository<TrafficParticipantEntity, TrafficParticipantEntityMapper>();
                var genericPacketRepository = uow.GetRepository<GenericPacketEntity, GenericPacketEntityMapper>();

                var resolvedParticipantIds = await trafficParticipantRepository.Get()
                    .Where(tp => tp.FileAnalysisId == fileAnalysisId && resolvedIpAddresses.Contains(tp.Address))
                    .Select(tp => tp.Id)
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (resolvedParticipantIds.Any())
                {
                    var relatedPackets = await genericPacketRepository.Get()
                        .Where(gp => gp.FileAnalysisId == fileAnalysisId &&
                                     (resolvedParticipantIds.Contains(gp.SenderId) ||
                                      resolvedParticipantIds.Contains(gp.RecipientId)))
                        .Include(gp => gp.Sender)
                        .Include(gp => gp.Recipient)
                        .ToListAsync()
                        .ConfigureAwait(false);

                    var communicatingParticipantDetails = new HashSet<string>();
                    foreach (var packet in relatedPackets)
                    {
                        if (resolvedParticipantIds.Contains(packet.SenderId) && packet.Recipient != null)
                        {
                            if (!resolvedParticipantIds.Contains(packet.Recipient.Id))
                            {
                                communicatingParticipantDetails.Add(
                                    $"{packet.Recipient.Address}:{packet.Recipient.Port}");
                            }
                        }

                        else if (resolvedParticipantIds.Contains(packet.RecipientId) && packet.Sender != null)
                        {
                            if (!resolvedParticipantIds.Contains(packet.Sender.Id))
                            {
                                communicatingParticipantDetails.Add($"{packet.Sender.Address}:{packet.Sender.Port}");
                            }
                        }
                    }

                    communicatingEndpoints = communicatingParticipantDetails.ToList();
                }
            }

            result.Add(new KnownServiceDataModel
            {
                DomainName = kd.DomainName,
                DomainPurpose = kd.Purpose,
                DomainDescription = kd.Description,
                ProductName = kd.CryptoProduct?.ProductName ?? "N/A",
                ProductVendor = kd.CryptoProduct?.Vendor ?? "N/A",
                QueryingEndpoints = queryingEndpoints!,
                ResolvedIpAddresses = resolvedIpAddresses!,
                CommunicatingEndpoints = communicatingEndpoints
            });
        }

        return result;
    }
}
