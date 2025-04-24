namespace CryTraCtor.Business.Models.Agregates
{
    public class TrafficParticipantKnownDomainSummaryModel : IModel
    {
        public Guid Id { get; set; }
        public Guid TrafficParticipantId { get; set; }
        public string TrafficParticipantName { get; set; } = string.Empty;
        public List<ProductDomainSummary> ProductSummaries { get; set; } = [];
    }
}
