namespace CryTraCtor.Packet.Models;

public record DnsResourceRecordModel(string Name,string Ttl,  string RecordClass, string RecordType, string RecordData)
{
    public string Name { get; set; } = Name;
    public string Ttl { get; set; } = Ttl;
    public string RecordClass { get; set; } = RecordClass;
    public string RecordType { get; set; } = RecordType;
    public string RecordData { get; set; } = RecordData;
}