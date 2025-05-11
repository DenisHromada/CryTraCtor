namespace CryTraCtor.Business.Models.Aggregates
{
    public class ProductDomainSummary
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public List<PurposeDomainSummary> PurposeSummaries { get; set; } = [];
        public List<LinkedGenericPacketModel> LinkedGenericPackets { get; set; } = [];
    }
}
