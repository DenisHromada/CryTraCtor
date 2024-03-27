// This Class serves as an abstraction over the Kaitai generated DnsPacket class
using Kaitai;

namespace CryTraCtor.DataStructures;

public class DnsPacket(KaitaiStream p__io, KaitaiStruct p__parent = null, Kaitai.KaitaiDnsPacket p__root = null)
    : Kaitai.KaitaiDnsPacket(p__io, p__parent, p__root)
{
    
    public string GetFullyQualifiedDomainName(DnsPacket.DomainName kaitaiDomainName)
    {
        var domainName = string.Empty;
        foreach (var subDomain in kaitaiDomainName.Name)
        {
            if (subDomain.IsPointer)
            {
                var actualName = subDomain.Pointer.Contents;
                domainName += GetFullyQualifiedDomainName(actualName);
                break;
            }
            domainName += subDomain.Name + ".";
        }
        return domainName.TrimEnd('.');
    }
}
    