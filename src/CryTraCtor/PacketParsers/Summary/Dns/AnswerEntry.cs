namespace CryTraCtor.PacketParsers.Summary.Dns;

public record AnswerEntry(
    string Name,
    string Type,
    string Class,
    TimeSpan TimeToLive,
    string Address
) : QueryEntry(Name, Type, Class);