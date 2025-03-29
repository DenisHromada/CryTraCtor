using CryTraCtor.Business.Models.StoredFiles;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface IStoredFileFacade
{
    Task<List<StoredFileListModel>> GetAllAsync();
    Task<StoredFileDetailModel> GetFileMetadataAsync(string filename);
    Task<string> StoreAsync(StoredFileCreateModel createModel, Stream stream);
    Task<string> RenameAsync(string oldFilename, string newFilename);
    Task DeleteAsync(string filename);
}
