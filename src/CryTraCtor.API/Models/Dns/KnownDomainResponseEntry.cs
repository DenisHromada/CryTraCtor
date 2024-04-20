using System.Collections.ObjectModel;
using CryTraCtor.DataTypes;
using CryTraCtor.Models;

namespace CryTraCtor.APi.Models.Dns;

public record KnownDomainResponseEntry(
    string DomainName,
    string Vendor,
    string Product,
    string? Purpose,
    string? Cryptocurrency,
    string? Description,
    Collection<string> DnsAnswerAddresses
) : KnownDomainDetail(DomainName, Vendor, Product, Purpose, Cryptocurrency, Description);