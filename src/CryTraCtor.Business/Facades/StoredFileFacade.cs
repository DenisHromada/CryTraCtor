using CryTraCtor.Business.Facades.Interfaces;
using CryTraCtor.Business.Mappers;
using CryTraCtor.Business.Models.StoredFiles;
using CryTraCtor.Database.Repositories;
using Microsoft.AspNetCore.Http;

namespace CryTraCtor.Business.Facades;

public class StoredFileFacade(
    IStoredFileRepository storedFileRepository,
    StoredFileModelMapper modelMapper
) : IStoredFileFacade
{
    public List<StoredFileListModel> GetAll()
    {
        var storedFileEntities = storedFileRepository.GetAll();
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
        var storedFileEntity = await storedFileRepository.GetByFilenameAsync(filename);
        return modelMapper.MapToDetailModel(storedFileEntity);
    }

    public async Task<string> Store(IFormFile file)
    {
        var storedFileEntity = await storedFileRepository.InsertAsync(file);
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