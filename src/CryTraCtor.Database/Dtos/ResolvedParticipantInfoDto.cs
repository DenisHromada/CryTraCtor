namespace CryTraCtor.Database.Dtos;

public class ResolvedParticipantInfoDto
{
    public Guid DnsMessageId { get; set; }
    public Guid ParticipantId { get; set; }
    public required string Address { get; set; }
    public int Port { get; set; }
}
