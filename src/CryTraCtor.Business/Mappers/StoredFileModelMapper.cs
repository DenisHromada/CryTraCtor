using CryTraCtor.Business.Mappers.MapperBase;
using CryTraCtor.Business.Models.StoredFiles;
using CryTraCtor.Database.Entities;

namespace CryTraCtor.Business.Mappers;

public class StoredFileModelMapper : ModelMapperBase<StoredFileEntity, StoredFileListModel, StoredFileDetailModel>
{
    public override StoredFileListModel MapToListModel(StoredFileEntity? entity)
        => entity is null
            ? StoredFileListModel.Empty()
            : new StoredFileListModel
            {
                Id = entity.Id,
                PublicFileName = entity.PublicFileName,
                InternalFilePath = entity.InternalFilePath,
                MimeType = entity.MimeType,
                FileSize = entity.FileSize
            };

    public override StoredFileDetailModel MapToDetailModel(StoredFileEntity? entity)
        => entity is null
            ? StoredFileDetailModel.Empty()
            : new StoredFileDetailModel
            {
                Id = entity.Id,
                PublicFileName = entity.PublicFileName,
                InternalFilePath = entity.InternalFilePath,
                MimeType = entity.MimeType,
                FileSize = entity.FileSize
            };

    public override StoredFileEntity MapToEntity(StoredFileDetailModel model)
        => new()
        {
            Id = model.Id,
            PublicFileName = model.PublicFileName,
            InternalFilePath = model.InternalFilePath,
            MimeType = model.MimeType,
            FileSize = model.FileSize
        };

    public StoredFileEntity MapCreateModelToEntity(StoredFileCreateModel createModel)
    {
        return new StoredFileEntity
        {
            Id = Guid.Empty,
            InternalFilePath = string.Empty,

            PublicFileName = createModel.PublicFileName,
            MimeType = createModel.MimeType,
            FileSize = createModel.FileSize
        };
    }
}
