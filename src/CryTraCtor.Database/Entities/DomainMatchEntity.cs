using System.ComponentModel.DataAnnotations.Schema;
using CryTraCtor.Database.Enums;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities
{
    [PrimaryKey(nameof(KnownDomainId), nameof(DnsPacketId))]
    public class DomainMatchEntity
    {
        [ForeignKey(nameof(KnownDomainId))]
        public required Guid KnownDomainId { get; set; }
        public virtual KnownDomainEntity? KnownDomain { get; set; }

        [ForeignKey(nameof(DnsPacketId))]
        public required Guid DnsPacketId { get; set; }
        public virtual DnsPacketEntity? DnsPacket { get; set; }

        public required DomainMatchType MatchType { get; set; }
    }
}
