using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CryTraCtor.Database.Entities;

[Index(nameof(Address), nameof(Port), nameof(FileAnalysisId), IsUnique = true)]
public class TrafficParticipantEntity : IEntity
{
    public required Guid Id { get; set; }

    public string Address { get; set; } = string.Empty;
    public int Port { get; set; }

    public Guid FileAnalysisId { get; set; }
    public FileAnalysisEntity? FileAnalysis { get; set; }

    [InverseProperty(nameof(DnsMessageEntity.Sender))]
    public ICollection<DnsMessageEntity>? SentDnsMessages { get; set; } = new List<DnsMessageEntity>();

    [InverseProperty(nameof(DnsMessageEntity.Recipient))]
    public ICollection<DnsMessageEntity>? ReceivedDnsMessages { get; set; } = new List<DnsMessageEntity>();

    public ICollection<DnsMessageResolvedTrafficParticipantEntity> DnsMessagesResolvingThisParticipant { get; set; } = new List<DnsMessageResolvedTrafficParticipantEntity>();
}
