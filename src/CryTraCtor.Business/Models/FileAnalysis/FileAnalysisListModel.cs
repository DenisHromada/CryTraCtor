namespace CryTraCtor.Business.Models.FileAnalysis;

public class FileAnalysisListModel : IModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public Guid StoredFileId { get; set; }

    public static FileAnalysisListModel Empty() => new();
}
