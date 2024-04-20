namespace CryTraCtor.Packet.Models;

public record DnsResourceRecordModel(string Name,string Ttl,  string RecordClass, string RecordType, string RecordData)
{
    public string Name = Name;
    public string Ttl = Ttl;
    public string RecordClass = RecordClass;
    public string RecordType = RecordType;
    public string RecordData = RecordData;
}