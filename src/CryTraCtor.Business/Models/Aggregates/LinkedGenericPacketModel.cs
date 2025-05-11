namespace CryTraCtor.Business.Models.Aggregates
{
    public class LinkedGenericPacketModel
    {
        public DateTime Timestamp { get; set; }
        public string SourceIp { get; set; } = string.Empty;
        public int SourcePort { get; set; }
        public string DestinationIp { get; set; } = string.Empty;
        public int DestinationPort { get; set; }
        public string MatchedDomain { get; set; } = string.Empty;
        public string DnsQueryDomain { get; set; } = string.Empty;
        public string TrafficDirection { get; set; } = string.Empty;
    }
}
