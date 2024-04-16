using CryTraCtor.Models.StoredFiles;
using Microsoft.AspNetCore.Http;

namespace CryTraCtor.Facades;

public interface IStoredFileFacade : IFacade
{
    List<StoredFileListModel> GetAll();
    Task<StoredFileDetailModel> GetFileMetadataAsync(string filename);
    Task<string> Store(IFormFile file);
    // string Rename(string newFilename, string oldFilename);
    Task Delete(string filename);
}