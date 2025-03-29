using CryTraCtor.Business.Models.StoredFiles;

namespace CryTraCtor.Business.Models.TrafficParticipants;

public class TrafficParticipantDetailModel : IModel
{
    public Guid Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public int Port { get; set; }

    public StoredFileListModel StoredFile { get; set; } = StoredFileListModel.Empty();

    public static TrafficParticipantDetailModel Empty() => new();
}
