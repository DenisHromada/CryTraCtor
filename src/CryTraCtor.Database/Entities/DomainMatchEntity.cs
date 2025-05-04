using System.ComponentModel.DataAnnotations.Schema;
using CryTraCtor.Database.Enums;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities
{
    [PrimaryKey(nameof(KnownDomainId), nameof(DnsMessageId))]
    public class DomainMatchEntity
    {
        [ForeignKey(nameof(KnownDomainId))]
        public required Guid KnownDomainId { get; set; }
        public virtual KnownDomainEntity? KnownDomain { get; set; }

        [ForeignKey(nameof(DnsMessageId))]
        public required Guid DnsMessageId { get; set; }
        public virtual DnsMessageEntity? DnsMessage { get; set; }

        public required DomainMatchType MatchType { get; set; }
    }
}
