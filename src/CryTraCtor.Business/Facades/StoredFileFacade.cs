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

    public async Task<string> Store(StoredFileDetailModel detailModel, Stream stream)
    {
        var entity = modelMapper.MapToEntity(detailModel);
        var storedFileEntity = await storedFileRepository.InsertAsync(entity, stream);
        return storedFileEntity.PublicFileName;
    }

    // public string Rename(string newFilename, string oldFilename)
    // {
    //     var storedFileEntity = storedFileRepository.GetByFilename(oldFilename);
    //     throw new NotImplementedException();
    // }

    public async Task Delete(string publicFileName)
    {
        await storedFileRepository.DeleteAsync(publicFileName);
    }
}