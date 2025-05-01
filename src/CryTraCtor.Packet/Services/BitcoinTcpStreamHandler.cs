using PacketDotNet;
using PacketDotNet.Connections;
using SharpPcap;
using SharpPcap.LibPcap;
using CryTraCtor.Packet.Models;
using System.Collections.Concurrent; // Using ConcurrentQueue for thread safety if needed, though likely not strictly necessary here

namespace CryTraCtor.Packet.Services;

public class BitcoinTcpStreamHandler : IDisposable
{
    private readonly TcpConnectionManager _tcpConnectionManager = new();
    private ICaptureDevice? _device;
    private bool _disposed;
    private readonly ConcurrentQueue<TcpDataChunk> _dataChunkQueue = new();

    public BitcoinTcpStreamHandler()
    {
        _tcpConnectionManager.OnConnectionFound += HandleTcpConnectionManagerOnConnectionFound;
    }

    public IEnumerable<TcpDataChunk> GetDataChunksFromFile(string fileName)
    {
        _device = new CaptureFileReaderDevice(fileName);
        _device.Open();
        _device.Filter = "(ip or ip6) and tcp and port 8333";

        while (_device.GetNextPacket(out var packetCapture) == GetPacketStatus.PacketRead)
        {
            var rawPacket = packetCapture.GetPacket();
            var parsedPacket = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            var tcpPacket = parsedPacket.Extract<TcpPacket>();

            if (tcpPacket != null)
            {
                _tcpConnectionManager.ProcessPacket(rawPacket.Timeval, tcpPacket);

                // Dequeue and yield any data chunks added by event handlers
                while (_dataChunkQueue.TryDequeue(out var chunk))
                {
                    yield return chunk;
                }
            }
        }

        _device.Close();

        // Yield any remaining chunks after processing all packets
        while (_dataChunkQueue.TryDequeue(out var chunk))
        {
            yield return chunk;
        }
    }

    private void HandleTcpConnectionManagerOnConnectionFound(TcpConnection connection)
    {
        var flow0 = connection.Flows[0];
        var flow1 = connection.Flows[1];

        var flow0SourceEndpoint = new InternetEndpointModel(flow0.address.ToString(), (ushort)flow0.port);
        var flow0DestinationEndpoint = new InternetEndpointModel(flow1.address.ToString(), (ushort)flow1.port);

        var flow1SourceEndpoint = new InternetEndpointModel(flow1.address.ToString(), (ushort)flow1.port);
        var flow1DestinationEndpoint = new InternetEndpointModel(flow0.address.ToString(), (ushort)flow0.port);

        flow0.OnPacketReceived += (timeval, tcp, conn, flow) =>
        {
            if (tcp?.PayloadData != null && tcp.PayloadData.Length > 0)
            {
                var timestamp = new DateTimeOffset(timeval.Date);
                _dataChunkQueue.Enqueue(new TcpDataChunk(flow, tcp.PayloadData, timestamp, flow0SourceEndpoint, flow0DestinationEndpoint));
            }
        };
        flow0.OnFlowClosed += (timeval, tcp, conn, flow) =>
        {
            var timestamp = new DateTimeOffset(timeval.Date);
            _dataChunkQueue.Enqueue(new TcpDataChunk(flow, ReadOnlyMemory<byte>.Empty, timestamp, flow0SourceEndpoint, flow0DestinationEndpoint, isFlowClosed: true));
        };

        flow1.OnPacketReceived += (timeval, tcp, conn, flow) =>
        {
            if (tcp?.PayloadData != null && tcp.PayloadData.Length > 0)
            {
                var timestamp = new DateTimeOffset(timeval.Date);
                _dataChunkQueue.Enqueue(new TcpDataChunk(flow, tcp.PayloadData, timestamp, flow1SourceEndpoint, flow1DestinationEndpoint));
            }
        };
        flow1.OnFlowClosed += (timeval, tcp, conn, flow) =>
        {
            var timestamp = new DateTimeOffset(timeval.Date);
            _dataChunkQueue.Enqueue(new TcpDataChunk(flow, ReadOnlyMemory<byte>.Empty, timestamp, flow1SourceEndpoint, flow1DestinationEndpoint, isFlowClosed: true));
        };
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _tcpConnectionManager.OnConnectionFound -= HandleTcpConnectionManagerOnConnectionFound;
                if (_tcpConnectionManager is IDisposable disposableManager)
                {
                    disposableManager.Dispose();
                }
                _device?.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
