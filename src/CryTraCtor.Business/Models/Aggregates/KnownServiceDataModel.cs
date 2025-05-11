namespace CryTraCtor.Business.Models.Aggregates;

public class KnownServiceDataModel
{
    public required string DomainName { get; set; }
    public required string DomainPurpose { get; set; }
    public required string DomainDescription { get; set; }
    public required string ProductName { get; set; }
    public required string ProductVendor { get; set; }
    public required List<string> QueryingEndpoints { get; set; }
    public required List<string> ResolvedIpAddresses { get; set; }
    public required List<string> CommunicatingEndpoints { get; set; }
}
