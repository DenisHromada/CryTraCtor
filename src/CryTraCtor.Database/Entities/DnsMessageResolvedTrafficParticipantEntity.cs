using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities
{
    [PrimaryKey(nameof(DnsMessageId), nameof(TrafficParticipantId))]
    public class DnsMessageResolvedTrafficParticipantEntity
    {
        public required Guid DnsMessageId { get; set; }
        public DnsMessageEntity? DnsMessage { get; set; }

        public required Guid TrafficParticipantId { get; set; }
        public TrafficParticipantEntity? TrafficParticipant { get; set; }
    }
}
