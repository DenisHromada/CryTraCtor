namespace CryTraCtor.Database.Entities;

public class KnownDomainEntity : IEntity
{
    public Guid Id { get; set; }
    public string DomainName { get; set; } // domain name that matches
    public string Purpose { get; set; } // e.g. bitcoin blockbook, telemetry
    public string Description { get; set; } // additional information

    public Guid KnownProductId { get; set; }
    public CryptoProductEntity CryptoProduct { get; set; }
}