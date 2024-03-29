using System.Collections.ObjectModel;
using CryTraCtor.PacketParsers.Summary.Dns;
using Kaitai;

namespace CryTraCtor.PacketParsers.RawToSummaryMapper.Dns;

public class DnsPayloadWrapper
{
    private readonly KaitaiDnsPacket _dnsPacket;

    public DnsPayloadWrapper(byte[] dnsBytes)
    {
        var kaitaiStream = new KaitaiStream(dnsBytes);
        _dnsPacket = new KaitaiDnsPacket(kaitaiStream);
    }


    public uint GetTransactionId()
    {
        return _dnsPacket.TransactionId;
    }

    public Collection<QueryEntry> GetQueries()
    {
        var queries = new Collection<QueryEntry>();

        foreach (var query in _dnsPacket.Queries)
        {
            var parsedQuery = new QueryEntry(
                GetFullyQualifiedDomainName(query.Name),
                GetRecordType(query),
                GetRecordClass(query)
            );
            queries.Add(parsedQuery);
        }

        return queries;
    }

    public Collection<AnswerEntry> GetAnswers()
    {
        var answers = new Collection<AnswerEntry>();

        foreach (var answer in _dnsPacket.Answers)
        {
            var parsedAnswer = new AnswerEntry(
                GetFullyQualifiedDomainName(answer.Name),
                GetRecordType(answer),
                GetRecordClass(answer),
                GetRecordAddress(answer)
            );
            answers.Add(parsedAnswer);
        }

        return answers;
    }

    public DnsMessageType GetDnsMessageType()
    {
        // QR bit is 0 for query packets; 1 for response packets
        return _dnsPacket.Flags.Qr == 1 ? DnsMessageType.Response : DnsMessageType.Query;
    }

    private static string GetRecordType(KaitaiDnsPacket.Answer kaitaiDnsPacketAnswer) =>
        kaitaiDnsPacketAnswer.Type.ToString();

    private static string GetRecordType(KaitaiDnsPacket.Query kaitaiDnsPacketQuery) =>
        kaitaiDnsPacketQuery.Type.ToString();

    private static string GetRecordClass(KaitaiDnsPacket.Answer kaitaiDnsPacketAnswer) =>
        kaitaiDnsPacketAnswer.AnswerClass.ToString();

    private static string GetRecordClass(KaitaiDnsPacket.Query kaitaiDnsPacketQuery) =>
        kaitaiDnsPacketQuery.QueryClass.ToString();

    private static string GetRecordAddress(KaitaiDnsPacket.Answer answer)
    {
        var address = string.Empty;
        if (GetRecordType(answer) == KaitaiDnsPacket.TypeType.A.ToString()
            && GetRecordClass(answer) == KaitaiDnsPacket.ClassType.InClass.ToString())
        {
            var answerPayload = (KaitaiDnsPacket.Address)answer.Payload;
            foreach (var octet in answerPayload.Ip)
            {
                address += octet + ".";
            }
        }

        return address;
    }

    private static string GetFullyQualifiedDomainName(KaitaiDnsPacket.DomainName kaitaiDomainName)
    {
        var domainName = string.Empty;
        foreach (var subDomain in kaitaiDomainName.Name)
        {
            if (subDomain.IsPointer)
            {
                domainName = GetFullyQualifiedDomainName(subDomain.Pointer.Contents);
            }
            else
            {
                domainName += subDomain.Name + ".";
            }
        }

        return domainName.TrimEnd('.');
    }
}