namespace CryTraCtor.Packet.Summary.Dns;

public record QueryEntry(
    string Name,
    string Type,
    string Class
);