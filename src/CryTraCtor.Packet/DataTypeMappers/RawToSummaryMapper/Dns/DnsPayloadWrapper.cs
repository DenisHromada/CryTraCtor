using System.Collections.ObjectModel;
using CryTraCtor.Packet.DataTypes.Packet.Raw.Kaitai.Dns;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;
using CryTraCtor.Packet.Models;
using Kaitai;

namespace CryTraCtor.Packet.DataTypeMappers.RawToSummaryMapper.Dns;

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

    public DnsResourceRecordModel GetQuery()
    {
        if (_dnsPacket.Queries.Count != 1)
        {
            throw new ApplicationException("Expected exactly one query in dns packet.");
        }

        var query = _dnsPacket.Queries.First();

        var parsedQuery = new DnsResourceRecordModel(
            GetFullyQualifiedDomainName(query.Name),
            string.Empty,
            GetRecordClass(query),
            GetRecordType(query),
            string.Empty
        );

        return parsedQuery;
    }

    public Collection<DnsResourceRecordModel> GetAnswers()
    {
        var answers = new Collection<DnsResourceRecordModel>();

        foreach (var answer in _dnsPacket.Answers)
        {
            var parsedAnswer = new DnsResourceRecordModel(
                GetFullyQualifiedDomainName(answer.Name),
                GetRecordTimeToLive(answer),
                GetRecordClass(answer),
                GetRecordType(answer),
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
        if (GetRecordClass(answer) != KaitaiDnsPacket.ClassType.InClass.ToString())
        {
            return address;
        }

        if (GetRecordType(answer) == KaitaiDnsPacket.TypeType.A.ToString())
        {
            var answerPayload = (KaitaiDnsPacket.Address)answer.Payload;
            address = string.Join(".", answerPayload.Ip);
        } else if (GetRecordType(answer) == KaitaiDnsPacket.TypeType.Aaaa.ToString())
        {
            var answerPayload = (KaitaiDnsPacket.AddressV6)answer.Payload;
            address = string.Join(":", answerPayload.IpV6);
        }

        return address;
    }

    private static string GetRecordTimeToLive(KaitaiDnsPacket.Answer answer)
    {
        return answer.Ttl.ToString();
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
