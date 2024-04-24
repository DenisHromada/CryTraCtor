namespace CryTraCtor.Business.Models.KnownDomain;

public class KnownDomainImportModel
{
    public string Vendor { get; set; }
    public string ProductName { get; set; }
    
    public string DomainName { get; set; } 
    public string Purpose { get; set; }
    public string Description { get; set; }
    
    public static KnownDomainImportModel Empty()
        => new KnownDomainImportModel
        {
            Vendor = string.Empty,
            ProductName = string.Empty,
            DomainName = string.Empty,
            Purpose = string.Empty,
            Description = string.Empty
        };
}