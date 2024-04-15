using CryTraCtor.Database.Entities;

namespace CryTraCtor.Database.Repositories;

public class StoredFileRepository : IRepository<StoredFile>
{
    public IQueryable<StoredFile> Get()
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid entityId)
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> ExistsAsync(StoredFile entity)
    {
        throw new NotImplementedException();
    }

    public StoredFile Insert(StoredFile entity)
    {
        throw new NotImplementedException();
    }

    public Task<StoredFile> UpdateAsync(StoredFile entity)
    {
        throw new NotImplementedException();
    }
}