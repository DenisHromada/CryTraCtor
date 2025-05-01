using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers.TrafficParticipant;
using CryTraCtor.Business.Models.TrafficParticipants;
using CryTraCtor.Database.Entities;
using CryTraCtor.Business.Models.Agregates;
using CryTraCtor.Database.Enums;
using CryTraCtor.Database.Mappers;
using CryTraCtor.Database.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Business.Facades;

public class TrafficParticipantFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    TrafficParticipantModelMapper modelMapper
) : FacadeBase<
        TrafficParticipantEntity,
        TrafficParticipantListModel,
        TrafficParticipantDetailModel,
        TrafficParticipantEntityMapper>(unitOfWorkFactory, modelMapper),
    ITrafficParticipantFacade
{
    public async Task<IEnumerable<TrafficParticipantListModel>> GetByFileAnalysisIdAsync(Guid fileAnalysisId)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var aggregateRepository = uow.TrafficParticipantAggregates;

        var aggregateDtos = await aggregateRepository.GetAggregatedByFileAnalysisIdAsync(fileAnalysisId);

        var participantModels = aggregateDtos.Select(dto => new TrafficParticipantListModel
        {
            Id = dto.Id,
            Address = dto.Address,
            Port = dto.Port,
            FileAnalysisId = dto.FileAnalysisId,
            OutgoingDnsCount = dto.OutgoingDnsCount,
            IncomingDnsCount = dto.IncomingDnsCount,
            OutgoingBitcoinCount = dto.OutgoingBitcoinCount,
            IncomingBitcoinCount = dto.IncomingBitcoinCount,
            UniqueMatchedKnownDomainCount = dto.UniqueMatchedKnownDomainCount,
            TotalMatchedKnownDomainCount = dto.TotalMatchedKnownDomainCount
        }).OrderBy(p => p.Address).ThenBy(p => p.Port).ToList();

        return participantModels;
    }

    public async Task<TrafficParticipantKnownDomainSummaryModel?> GetKnownDomainSummaryAsync(Guid trafficParticipantId)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var participantRepo = uow.GetRepository<TrafficParticipantEntity, TrafficParticipantEntityMapper>();
        var domainMatchRepository = uow.DomainMatches;
        var knownDomainRepo = uow.GetRepository<KnownDomainEntity, KnownDomainEntityMapper>();

        var participant = await participantRepo.Get()
            .FirstOrDefaultAsync(p => p.Id == trafficParticipantId);

        if (participant == null)
        {
            return null;
        }

        var domainMatches = await domainMatchRepository.GetByTrafficParticipantIdAsync(trafficParticipantId);

        if (domainMatches.Count == 0)
        {
            return new TrafficParticipantKnownDomainSummaryModel
            {
                TrafficParticipantId = participant.Id,
                TrafficParticipantName = $"{participant.Address}:{participant.Port}"
            };
        }

        var summary = new TrafficParticipantKnownDomainSummaryModel
        {
            TrafficParticipantId = participant.Id,
            TrafficParticipantName = $"{participant.Address}:{participant.Port}"
        };


        var matchesByProduct = domainMatches
            .Where(dm => dm.KnownDomain?.CryptoProduct != null)
            .GroupBy(dm => dm.KnownDomain!.CryptoProduct!);

        foreach (var productGroup in matchesByProduct)
        {
            var product = productGroup.Key;
            var productSummary = new ProductDomainSummary
            {
                ProductId = product.Id,
                ProductName = product.ProductName
            };


            var matchesByPurpose = productGroup
                .Where(dm => dm.KnownDomain != null)
                .GroupBy(dm => dm.KnownDomain!.Purpose);

            foreach (var purposeGroup in matchesByPurpose)
            {
                var purpose = purposeGroup.Key;
                var purposeMatches = purposeGroup.ToList();

                var allKnownDomainsForProduct = await knownDomainRepo.Get()
                    .Where(kd => kd.CryptoProductId == product.Id && kd.Purpose == purpose)
                    .Select(kd => kd.DomainName)
                    .Distinct()
                    .ToListAsync();

                var purposeSummary = new PurposeDomainSummary
                {
                    Purpose = purpose,
                    TotalUniqueDomains =
                        allKnownDomainsForProduct.Count,

                    UniqueExactMatchCount = purposeMatches
                        .Where(dm => dm.MatchType == DomainMatchType.Exact)
                        .Select(dm => dm.KnownDomainId)
                        .Distinct()
                        .Count(),

                    TotalExactMatchCount = purposeMatches
                        .Count(dm => dm.MatchType == DomainMatchType.Exact),

                    UniqueSubdomainMatchCount = purposeMatches
                        .Where(dm => dm.MatchType == DomainMatchType.Subdomain)
                        .Select(dm => dm.KnownDomainId)
                        .Distinct()
                        .Count(),

                    TotalSubdomainMatchCount = purposeMatches
                        .Count(dm => dm.MatchType == DomainMatchType.Subdomain)
                };
                productSummary.PurposeSummaries.Add(purposeSummary);
            }

            summary.ProductSummaries.Add(productSummary);
        }

        return summary;
    }

    public async Task<TrafficParticipantDetailModel?> GetByAddressPortAndFileAnalysisIdAsync(Guid fileAnalysisId,
        string address, int port)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var participantRepo = uow.GetRepository<TrafficParticipantEntity, TrafficParticipantEntityMapper>();

        var entity = await participantRepo.Get()
            .FirstOrDefaultAsync(p => p.Address == address && p.Port == port && p.FileAnalysisId == fileAnalysisId);

        return entity == null ? null : ModelMapper.MapToDetailModel(entity);
    }
}
