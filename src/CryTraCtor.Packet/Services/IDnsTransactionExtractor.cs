using System.Collections.ObjectModel;
using CryTraCtor.Packet.Models;

namespace CryTraCtor.Packet.Services;

public interface IDnsTransactionExtractor
{
    Collection<DnsTransactionSummaryModel> Run(string fileName);
}