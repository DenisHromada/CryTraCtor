namespace CryTraCtor.Business.Models.KnownDomain;

public class KnownDomainListModel : IModel
{
    public Guid Id { get; set; }
    public string DomainName { get; set; } = string.Empty;

    public static KnownDomainListModel Empty()
        => new KnownDomainListModel
        {
            Id = Guid.Empty,
            DomainName = string.Empty
        };
}
