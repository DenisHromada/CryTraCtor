namespace CryTraCtor.Packet.Summary.Dns;

public record AnswerEntry(
    string Name,
    string Type,
    string Class,
    string Address
) : QueryEntry(Name, Type, Class);