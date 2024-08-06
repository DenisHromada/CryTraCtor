using CryTraCtor.Business.Models.StoredFiles;

namespace CryTraCtor.Business.Facades.Interfaces;

public interface IStoredFileFacade
{
    List<StoredFileListModel> GetAll();
    Task<StoredFileDetailModel> GetFileMetadataAsync(string filename);
    Task<string> Store(StoredFileDetailModel detailModel, Stream stream);
    Task<string> Rename(string oldFilename, string newFilename);
    Task Delete(string filename);
}
