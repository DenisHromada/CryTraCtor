// This Class serves as an abstraction over the Kaitai generated DnsPacket class

using System.Collections.ObjectModel;
using Kaitai;
using Microsoft.VisualBasic;

namespace CryTraCtor.DataStructures.Dns;

public class DnsPacket(KaitaiStream p__io, KaitaiStruct p__parent = null, Kaitai.KaitaiDnsPacket p__root = null)
    : Kaitai.KaitaiDnsPacket(p__io, p__parent, p__root)
{
    private static string GetFullyQualifiedDomainName(DomainName kaitaiDomainName)
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
    
    public Collection<DnsAnswer> GetQueries()
    {
        var queries = new Collection<DnsAnswer>();

        foreach (var query in Queries)
        {
            var parsedQuery = new DnsAnswer(GetFullyQualifiedDomainName(query.Name));
            queries.Add(parsedQuery);
        }

        return queries;
    }
    
    public Collection<DnsAnswer> GetAnswers()
    {
        var answers = new Collection<DnsAnswer>();

        foreach (var answer in Answers)
        {
            var parsedAnswer = new DnsAnswer(GetFullyQualifiedDomainName(answer.Name));
            answers.Add(parsedAnswer);
        }
        
        return answers;
    }
}
    