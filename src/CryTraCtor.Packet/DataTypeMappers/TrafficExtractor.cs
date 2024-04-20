namespace CryTraCtor.Packet.DataTypeMappers;

public abstract class TrafficExtractor(string analyzedFileName)
{
    protected string AnalyzedFileName { get; set; } = analyzedFileName;

    public abstract void Run();
}