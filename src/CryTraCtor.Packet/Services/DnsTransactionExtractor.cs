using System.Collections.ObjectModel;
using CryTraCtor.Packet.Mappers;
using CryTraCtor.Packet.Models;

namespace CryTraCtor.Packet.Services;

public class DnsTransactionExtractor(
    IDnsPacketReader dnsPacketReader,
    IDnsTrafficMapper dnsTrafficMapper
) : IDnsTransactionExtractor
{
    public Collection<DnsTransactionSummaryModel> Run(string fileName)
    {
        var dnsPackets = dnsPacketReader.Read(fileName);
        var dnsTransactions = dnsTrafficMapper.GetTransactions(dnsPackets);

        return dnsTransactions;
    }
}