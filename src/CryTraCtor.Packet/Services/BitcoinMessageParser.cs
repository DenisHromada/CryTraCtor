using System.Text;
using CryTraCtor.Packet.DataTypes.Packet.Summary.Bitcoin;
using CryTraCtor.Packet.Models;
using NBitcoin;
using NBitcoin.Crypto;
using PacketDotNet.Connections;
using Microsoft.Extensions.Logging;
using System.Buffers.Binary;

namespace CryTraCtor.Packet.Services;

public class BitcoinMessageParser(ILogger<BitcoinMessageParser> logger)
{
    private const int HeaderSize = 24;
    private const int MagicSize = 4;
    private const int CommandSize = 12;
    private const int LengthSize = 4;
    private const int MaxBufferSizeBeforeDiscard = 64 * 1024;
    private const uint MaxPayloadSize = 4 * 1024 * 1024;

    private class ParsingFlowState
    {
        public List<byte> Buffer { get; } = [];
        public InternetEndpointModel SourceEndpoint { get; set; }
        public InternetEndpointModel DestinationEndpoint { get; set; }
    }

    private class ReadOperationState
    {
        public Dictionary<TcpFlow, ParsingFlowState> FlowBuffers { get; } = new();
        public List<IBitcoinPacketSummary> Results { get; } = [];
    }

    public List<IBitcoinPacketSummary> Read(IEnumerable<TcpDataChunk> dataChunks)
    {
        var state = new ReadOperationState();

        foreach (var chunk in dataChunks)
        {
            if (chunk.IsFlowClosed)
            {
                state.FlowBuffers.Remove(chunk.Flow);
            }
            else
            {
                if (!state.FlowBuffers.TryGetValue(chunk.Flow, out var flowInfo))
                {
                    flowInfo = new ParsingFlowState
                    {
                        SourceEndpoint = chunk.SourceEndpoint,
                        DestinationEndpoint = chunk.DestinationEndpoint
                    };
                    state.FlowBuffers.Add(chunk.Flow, flowInfo);
                }


                flowInfo.Buffer.AddRange(chunk.Data.ToArray());


                while (flowInfo.Buffer.Count >= MagicSize)
                {
                    int magicIndex = FindMagicBytes(flowInfo.Buffer, Network.Main.Magic);

                    if (magicIndex == -1)
                    {
                        if (flowInfo.Buffer.Count > MaxBufferSizeBeforeDiscard)
                        {
                            int bytesToKeep = Math.Min(flowInfo.Buffer.Count, MagicSize - 1);
                            logger.LogWarning(
                                "Discarding {DiscardedBytes} bytes from buffer due to excessive size without magic bytes.",
                                flowInfo.Buffer.Count - bytesToKeep);
                            flowInfo.Buffer.RemoveRange(0, flowInfo.Buffer.Count - bytesToKeep);
                        }

                        break;
                    }

                    if (magicIndex > 0)
                    {
                        logger.LogDebug("Discarding {DiscardedBytes} bytes before magic bytes.", magicIndex);
                        flowInfo.Buffer.RemoveRange(0, magicIndex);

                        continue;
                    }


                    if (TryReadBitcoinMessage(flowInfo.Buffer, chunk.Timestamp, flowInfo, out var messageSummary,
                            out int consumedBytes,
                            out var failureReason, state))
                    {
                        state.Results.Add(messageSummary);
                        flowInfo.Buffer.RemoveRange(0, consumedBytes);
                    }
                    else
                    {
                        if (failureReason == ParseFailureReason.NeedMoreData)
                        {
                            break;
                        }
                        else
                        {
                            logger.LogWarning("Failed to parse message: {FailureReason}. Discarding magic bytes.",
                                failureReason);
                            flowInfo.Buffer.RemoveRange(0, MagicSize);
                        }
                    }
                }
            }
        }

        return state.Results;
    }

    private enum ParseFailureReason
    {
        NeedMoreData,
        InvalidMagic,
        InvalidChecksum,
        InvalidLength,
        Exception
    }


    private bool TryReadBitcoinMessage(List<byte> buffer, DateTimeOffset timestamp, ParsingFlowState flowInfo,
        out BitcoinMessageSummary? messageSummary, out int consumedBytes, out ParseFailureReason failureReason,
        ReadOperationState state)
    {
        messageSummary = null;
        consumedBytes = 0;
        failureReason = ParseFailureReason.NeedMoreData;

        if (buffer.Count < HeaderSize)
        {
            failureReason = ParseFailureReason.NeedMoreData;
            return false;
        }

        try
        {
            byte[] headerBytes = buffer.GetRange(0, HeaderSize).ToArray();
            uint magic = BinaryPrimitives.ReadUInt32LittleEndian(headerBytes.AsSpan());

            if (magic != Network.Main.Magic)
            {
                failureReason = ParseFailureReason.InvalidMagic;
                return false;
            }

            string command = Encoding.ASCII
                .GetString(headerBytes, MagicSize, CommandSize).TrimEnd('\0');
            uint length =
                BinaryPrimitives.ReadUInt32LittleEndian(
                    headerBytes.AsSpan(MagicSize + CommandSize));
            uint checksum = BinaryPrimitives.ReadUInt32LittleEndian(headerBytes.AsSpan(MagicSize +
                CommandSize + LengthSize));

            if (length > MaxPayloadSize)
            {
                failureReason = ParseFailureReason.InvalidLength;
                return false;
            }

            int totalMessageSize = HeaderSize + (int)length;

            if (buffer.Count < totalMessageSize)
            {
                failureReason = ParseFailureReason.NeedMoreData;
                return false;
            }

            byte[] payloadBytes = buffer.GetRange(HeaderSize, (int)length).ToArray();
            uint calculatedChecksum = CalculateChecksum(payloadBytes);

            if (checksum != calculatedChecksum)
            {
                failureReason = ParseFailureReason.InvalidChecksum;
                return false;
            }

            messageSummary = new BitcoinMessageSummary
            {
                Timestamp = timestamp.UtcDateTime,
                Source = new InternetEndpointModel(flowInfo.SourceEndpoint.Address, flowInfo.SourceEndpoint.Port),
                Destination = new InternetEndpointModel(flowInfo.DestinationEndpoint.Address,
                    flowInfo.DestinationEndpoint.Port),
                Command = command,
                PayloadSize = length,
                Magic = magic,
                Checksum = checksum
            };

            consumedBytes = totalMessageSize;
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(0, ex, "Error parsing Bitcoin message.");
            failureReason = ParseFailureReason.Exception;
            return false;
        }
    }

    private static int FindMagicBytes(List<byte> buffer, uint magic)
    {
        var magicBytes = BitConverter.GetBytes(magic);
        if (!BitConverter.IsLittleEndian) Array.Reverse(magicBytes);

        for (int i = 0; i <= buffer.Count - magicBytes.Length; i++)
        {
            bool found = true;
            for (int j = 0; j < magicBytes.Length; j++)
            {
                if (buffer[i + j] != magicBytes[j])
                {
                    found = false;
                    break;
                }
            }

            if (found) return i;
        }

        return -1;
    }

    private static uint CalculateChecksum(byte[] payload)
    {
        var hash = Hashes.DoubleSHA256(payload);
        return BinaryPrimitives.ReadUInt32LittleEndian(hash.ToBytes().AsSpan());
    }
}
