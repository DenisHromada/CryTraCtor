using CryTraCtor.Business.Models.FileAnalysis;

namespace CryTraCtor.Business.Models.TrafficParticipants;

public class TrafficParticipantDetailModel : IModel
{
    public Guid Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public int Port { get; set; }

    public FileAnalysisListModel FileAnalysis { get; set; } = FileAnalysisListModel.Empty();

    public ICollection<DnsPacketModel> SentDnsPackets { get; set; } = new List<DnsPacketModel>();
    public ICollection<DnsPacketModel> ReceivedDnsPackets { get; set; } = new List<DnsPacketModel>();

    public static TrafficParticipantDetailModel Empty() => new();
}
