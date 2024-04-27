using System.Collections.ObjectModel;
using CryTraCtor.Packet.DataTypes.DnsTransaction;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Dns;
using CryTraCtor.Packet.Models;

namespace CryTraCtor.Packet.Mappers;

public class DnsTrafficMapper : IDnsTrafficMapper
{
    public Collection<DnsTransactionSummaryModel> GetTransactions(IEnumerable<IDnsPacketSummary> dnsPackets)
    {
        var dnsTransactionIdDictionary = GroupByTxId(dnsPackets);

        var dnsTransactions = MapDictToTxs(dnsTransactionIdDictionary);

        return dnsTransactions;
    }

    private static Dictionary<uint, DnsTransactionTraffic> GroupByTxId(IEnumerable<IDnsPacketSummary> dnsPackets)
    {
        var dnsTransactionDictionary = new Dictionary<uint, DnsTransactionTraffic>();
        foreach (var dnsPacket in dnsPackets)
        {
            if (dnsTransactionDictionary.TryGetValue(dnsPacket.TransactionId, out var value))
            {
                value.AddDnsSummary(dnsPacket);
            }
            else
            {
                var newDnsTransaction =
                    new DnsTransactionTraffic(new Collection<DnsPacketResponse>(), new Collection<DnsPacketQuery>());
                newDnsTransaction.AddDnsSummary(dnsPacket);
                dnsTransactionDictionary.Add(dnsPacket.TransactionId, newDnsTransaction);
            }
        }

        return dnsTransactionDictionary;
    }

    private static Collection<DnsTransactionSummaryModel> MapDnsTransactionTrafficToTransactions(
        DnsTransactionTraffic dnsTraffic
    )
    {
        var dnsTransactions = new Collection<DnsTransactionSummaryModel>();

        // Duplicate TxId's are common in larger datasets
        // The following deduplication is flawed in a bunch of ways
        // For example doesn't account for duplicate requests
        foreach (var query in dnsTraffic.Queries)
        {
            foreach (var response in dnsTraffic.Responses)
            {
                if (QueryMatchesResponse(query, response))
                {
                    var transactionSummary = new DnsTransactionSummaryModel(
                        response.TransactionId,
                        query.Source,
                        query.Destination,
                        query.Query,
                        response.Answers
                    );

                    dnsTransactions.Add(transactionSummary);
                }
            }
        }

        return dnsTransactions;
    }

    private static bool QueryMatchesResponse(DnsPacketQuery packetQuery, DnsPacketResponse packetResponse)
    {
        return (packetQuery.TransactionId == packetResponse.TransactionId
                && packetQuery.Source == packetResponse.Destination
                && packetQuery.Query == packetResponse.Query
            );
    }

    private static Collection<DnsTransactionSummaryModel> MapDictToTxs(
        Dictionary<uint, DnsTransactionTraffic> transactionDictionary)
    {
        Collection<DnsTransactionSummaryModel> dnsTransactions = [];
        foreach (var transactionDictPair in transactionDictionary)
        {
            var dnsTransactionSummaries =
                MapDnsTransactionTrafficToTransactions(transactionDictPair.Value);
            foreach (var transactionSummary in dnsTransactionSummaries)
            {
                dnsTransactions.Add(transactionSummary);
            }
        }

        return dnsTransactions;
    }
}