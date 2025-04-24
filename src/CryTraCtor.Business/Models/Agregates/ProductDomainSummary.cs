namespace CryTraCtor.Business.Models.Agregates
{
    public class ProductDomainSummary
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public List<PurposeDomainSummary> PurposeSummaries { get; set; } = new();
    }
}
