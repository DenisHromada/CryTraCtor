using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers;
using CryTraCtor.Business.Models.StoredFiles;
using CryTraCtor.Database.Repositories;

namespace CryTraCtor.Business.Facades;

public class StoredFileFacade(
    IStoredFileRepository storedFileRepository,
    StoredFileModelMapper modelMapper
) : IStoredFileFacade
{
    public List<StoredFileListModel> GetAll()
    {
        var storedFileEntities = storedFileRepository.GetMetadataAll();
        var storedFileListModels = new List<StoredFileListModel>();
        foreach (var entity in storedFileEntities)
        {
            var model = modelMapper.MapToListModel(entity);
            storedFileListModels.Add(model);
        }

        return storedFileListModels;
    }
    public async Task<StoredFileDetailModel> GetFileMetadataAsync(string filename)
    {
        var storedFileEntity = await storedFileRepository.GetMetadataByFilenameAsync(filename);
        return modelMapper.MapToDetailModel(storedFileEntity);
    }

    public async Task<string> StoreAsync(StoredFileDetailModel detailModel, Stream stream)
    {
        var entity = modelMapper.MapToEntity(detailModel);
        var storedFileEntity = await storedFileRepository.InsertAsync(entity, stream);
        return storedFileEntity.PublicFileName;
    }

    public async Task<string> Rename(string oldFilename, string newFilename)
    {
        return (await storedFileRepository.RenameAsync(oldFilename, newFilename)).PublicFileName;
    }

    public async Task Delete(string publicFileName)
    {
        await storedFileRepository.DeleteAsync(publicFileName);
    }
}
