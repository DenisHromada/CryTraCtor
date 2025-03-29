namespace CryTraCtor.Business.Models.TrafficParticipants;

public class TrafficParticipantListModel : IModel
{
    public Guid Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public int Port { get; set; }
    public Guid StoredFileId { get; set; }

    public static TrafficParticipantListModel Empty() => new();
}
