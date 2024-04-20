using CryTraCtor.Business.Models.StoredFiles;
using Microsoft.AspNetCore.Http;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface IStoredFileFacade 
{
    List<StoredFileListModel> GetAll();
    Task<StoredFileDetailModel> GetFileMetadataAsync(string filename);
    Task<string> Store(IFormFile file);
    // string Rename(string newFilename, string oldFilename);
    Task Delete(string filename);
}