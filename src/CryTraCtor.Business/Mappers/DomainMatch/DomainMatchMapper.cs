using CryTraCtor.Business.Models.DomainMatch;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers.DomainMatch
{
    public class DomainMatchMapper
    {
        public DomainMatchEntity MapModelToEntity(DomainMatchModel model)
        {
            return new DomainMatchEntity
            {
                KnownDomainId = model.KnownDomainId,
                DnsPacketId = model.DnsPacketId,
                MatchType = model.MatchType
            };
        }
    }
}
