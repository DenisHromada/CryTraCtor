using System.Collections.ObjectModel;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;
using CryTraCtor.Packet.Models;

namespace CryTraCtor.Packet.Mappers;

public interface IDnsTrafficMapper
{
    Collection<DnsTransactionSummaryModel> GetTransactions(IEnumerable<IDnsPacketSummary> dnsPackets);
}