using System.Collections.ObjectModel;
using CryTraCtor.Packet.DataTypeMappers;
using CryTraCtor.Packet.DataTypeMappers.RawToSummaryMapper.Dns;
using CryTraCtor.Packet.DataTypes.DnsTransaction;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;
using CryTraCtor.Packet.Models;
using SharpPcap;

namespace CryTraCtor.Packet.Services;

public class DnsTransactionExtractor(
    IDnsPacketReader dnsPacketReader
    ) : IDnsTransactionExtractor
{
    private Collection<DnsTransactionSummaryModel> DnsTransactions { get; set; } = [];
    private Dictionary<uint, DnsTransactionTraffic> _dnsTransactionDictionary = new();

    public Collection<DnsTransactionSummaryModel> Run(string fileName)
    {
        var dnsPackets = dnsPacketReader.Read(fileName);

        foreach (var dnsPacket in dnsPackets)
        {
            AddPacketDnsSummaryToTransactions(dnsPacket);
        }
        
        foreach (var transactionDictPair in _dnsTransactionDictionary)
        {
            var dnsTransactionSummaries =
                DnsTrafficToTransaction.MapDnsTrafficToTransactions(transactionDictPair.Value);
            foreach (var transactionSummary in dnsTransactionSummaries)
            {
                DnsTransactions.Add(transactionSummary);
            }
        }

        return DnsTransactions;
    }


    private void AddPacketDnsSummaryToTransactions(IDnsPacketSummary dnsPacketSummary)
    {
        if (_dnsTransactionDictionary.TryGetValue(dnsPacketSummary.TransactionId, out var value))
        {
            value.AddDnsSummary(dnsPacketSummary);
        }
        else
        {
            var newDnsTransaction =
                new DnsTransactionTraffic(new Collection<DnsPacketResponse>(), new Collection<DnsPacketQuery>());
            newDnsTransaction.AddDnsSummary(dnsPacketSummary);
            _dnsTransactionDictionary.Add(dnsPacketSummary.TransactionId, newDnsTransaction);
        }
    }
}