using CryTraCtor.Database.Entities;
using CryTraCtor.Mappers.ModelMapperBase;
using CryTraCtor.Models.StoredFiles;

namespace CryTraCtor.Mappers;

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
}