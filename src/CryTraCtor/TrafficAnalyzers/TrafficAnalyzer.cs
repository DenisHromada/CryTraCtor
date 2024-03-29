namespace CryTraCtor.TrafficAnalyzers;

public abstract class TrafficAnalyzer(string analyzedFileName)
{
    protected string AnalyzedFileName { get; set; } = analyzedFileName;

    public abstract void Run();
}