using System.Collections.ObjectModel;
using CryTraCtor.DataTypes.DnsTransaction;
using CryTraCtor.DataTypes.Packet.Summary.Dns;

namespace CryTraCtor.DataTypeMappers;

public static class DnsTrafficToTransaction
{
    public static Collection<DnsTransactionSummary> MapDnsTrafficToTransactions(DnsTransactionTraffic dnsTraffic)
    {
        var dnsTransactions = new Collection<DnsTransactionSummary>();

        if (dnsTraffic.Queries.Count != 1 || dnsTraffic.Responses.Count != 1)
        {
            return dnsTransactions;
            // throw new NotImplementedException("Found queries/responses with identical DNS transaction id's.");
        }

        var query = dnsTraffic.Queries.First();
        var response = dnsTraffic.Responses.First();

        if (!QueryMatchesResponse(query, response))
        {
            throw new ApplicationException("Query does not match response.");
        }

        var transactionSummary = new DnsTransactionSummary(
            response.TransactionId,
            query.Source,
            query.Destination,
            query.Query,
            response.Answers
        );
        
        dnsTransactions.Add(transactionSummary);

        return dnsTransactions;
    }

    private static bool QueryMatchesResponse(DnsQuery query, DnsResponse response)
    {
        return (query.TransactionId == response.TransactionId
                && query.Source == response.Destination
                && query.Query == response.Query
            );
    }
}