namespace CryTraCtor.Business.Models.Aggregates
{
    public class PurposeDomainSummary
     {
         public string Purpose { get; set; } = string.Empty;
         public int TotalUniqueDomains { get; set; }
         public int UniqueExactMatchCount { get; set; }
         public int TotalExactMatchCount { get; set; }
         public int UniqueSubdomainMatchCount { get; set; }
         public int TotalSubdomainMatchCount { get; set; }
     }
 }
