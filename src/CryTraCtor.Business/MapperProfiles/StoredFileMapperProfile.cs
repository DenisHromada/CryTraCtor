using AutoMapper;
using CryTraCtor.Database.Entities;
using CryTraCtor.Models.StoredFiles;

namespace CryTraCtor.MapperProfiles;

public class StoredFileMapperProfile : Profile
{
    public StoredFileMapperProfile()
    {
        CreateMap<StoredFileEntity, StoredFileDetailModel>();
    }
}