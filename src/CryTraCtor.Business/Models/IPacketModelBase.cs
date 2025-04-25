namespace CryTraCtor.Business.Models;

public interface IPacketModelBase : IModel
{
    Guid FileAnalysisId { get; set; }
    Guid SenderId { get; set; }
    Guid RecipientId { get; set; }
    DateTime Timestamp { get; set; }
}
