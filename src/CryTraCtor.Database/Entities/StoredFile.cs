using System.ComponentModel.DataAnnotations;

namespace CryTraCtor.Database.Entities;

public partial class StoredFile : IEntity
{
    [Key] public required Guid Id { get; set; }

    public string FilePath { get; set; } = null!;
}