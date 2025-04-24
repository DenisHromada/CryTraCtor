using CryTraCtor.Database.Enums;
namespace CryTraCtor.Business.Models.DomainMatch
{
    public class DomainMatchModel
    {
        public Guid KnownDomainId { get; set; }
        public Guid DnsPacketId { get; set; }
        public DomainMatchType MatchType { get; set; }
    }
}
