using CryTraCtor.Business.Models.TrafficParticipants;

namespace CryTraCtor.Business.Models;

public class BitcoinPacketDetailModel : IPacketModelBase
{
    public Guid Id { get; set; }

    public Guid FileAnalysisId { get; set; }
    public Guid SenderId { get; set; }
    public Guid RecipientId { get; set; }
    public DateTime Timestamp { get; set; }
    public uint Magic { get; set; }
    public string Command { get; set; }
    public uint Length { get; set; }
    public uint Checksum { get; set; }

    public TrafficParticipantListModel? Sender { get; set; }
    public TrafficParticipantListModel? Recipient { get; set; }
}
