using System.ComponentModel.DataAnnotations;

namespace CryTraCtor.Database.Entities;

public partial class StoredFile
{
    [Key] public Guid Id { get; set; }

    public string FilePath { get; set; } = null!;
}