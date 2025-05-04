using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers;
using CryTraCtor.Business.Models.StoredFiles;
using CryTraCtor.Database.Repositories;
using CryTraCtor.Database.Repositories.Interfaces;

namespace CryTraCtor.Business.Facades;

public class StoredFileFacade(
    IStoredFileRepository storedFileRepository,
    StoredFileModelMapper modelMapper
) : IStoredFileFacade
{
    public async Task<List<StoredFileListModel>> GetAllAsync()
    {
        var storedFileEntities = await storedFileRepository.GetMetadataAllAsync();
        return storedFileEntities.Select(modelMapper.MapToListModel).ToList();
    }

    public async Task<StoredFileDetailModel?> GetByIdAsync(Guid id)
    {
        var storedFileEntity = await storedFileRepository.GetMetadataByIdAsync(id);
        return modelMapper.MapToDetailModel(storedFileEntity);
    }

    public async Task<StoredFileDetailModel?> GetFileMetadataAsync(string filename)
    {
        var storedFileEntity = await storedFileRepository.GetMetadataByFilenameAsync(filename);
        return modelMapper.MapToDetailModel(storedFileEntity);
    }

    public async Task<string> StoreAsync(StoredFileCreateModel createModel, Stream stream)
    {
        var entity = modelMapper.MapCreateModelToEntity(createModel);
        var storedFileEntity = await storedFileRepository.InsertAsync(entity, stream);
        return storedFileEntity.PublicFileName;
    }

    public async Task<string> RenameAsync(string oldFilename, string newFilename)
    {
        return (await storedFileRepository.RenameAsync(oldFilename, newFilename)).PublicFileName;
    }

    public async Task DeleteAsync(string publicFileName)
    {
        await storedFileRepository.DeleteAsync(publicFileName);
    }
}
