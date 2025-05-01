using PacketDotNet.Connections;

namespace CryTraCtor.Packet.Models
{
    public class TcpDataChunk(
        TcpFlow flow,
        ReadOnlyMemory<byte> data,
        DateTimeOffset timestamp,
        InternetEndpointModel sourceEndpoint,
        InternetEndpointModel destinationEndpoint,
        bool isFlowClosed = false)
    {
        public TcpFlow Flow { get; } = flow;
        public ReadOnlyMemory<byte> Data { get; } = data;
        public DateTimeOffset Timestamp { get; } = timestamp;
        public InternetEndpointModel SourceEndpoint { get; } = sourceEndpoint;
        public InternetEndpointModel DestinationEndpoint { get; } = destinationEndpoint;
        public bool IsFlowClosed { get; } = isFlowClosed;
    }
}
