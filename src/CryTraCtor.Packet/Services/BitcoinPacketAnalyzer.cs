using CryTraCtor.Packet.DataTypes.Packet.Summary.Bitcoin;
using Microsoft.Extensions.Logging;

namespace CryTraCtor.Packet.Services;

public class BitcoinPacketAnalyzer(
    BitcoinMessageParser bitcoinMessageParser,
    ILogger<BitcoinPacketAnalyzer> logger,
    BitcoinTcpStreamHandler tcpStreamHandler)
{
    public IEnumerable<IBitcoinPacketSummary> AnalyzeFromFile(string filePath)
    {
        try
        {
            var dataChunks = tcpStreamHandler.GetDataChunksFromFile(filePath);
            return bitcoinMessageParser.Read(dataChunks);
        }
        catch (Exception ex)
        {
            logger.LogError(0, ex, "Error analyzing Bitcoin packets from file: {FilePath}", filePath);
            return [];
        }
    }
}
