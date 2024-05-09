using CryTraCtor.Business.Models.StoredFiles;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface IStoredFileFacade 
{
    List<StoredFileListModel> GetAll();
    Task<StoredFileDetailModel> GetFileMetadataAsync(string filename);
    Task<string> Store(StoredFileDetailModel detailModel, Stream stream);
    // string Rename(string newFilename, string oldFilename);
    Task Delete(string filename);
}