namespace CryTraCtor.Packet;

public abstract class TrafficExtractor(string analyzedFileName)
{
    protected string AnalyzedFileName { get; set; } = analyzedFileName;

    public abstract void Run();
}