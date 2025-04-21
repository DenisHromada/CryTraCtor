using CryTraCtor.Business.Models;
using CryTraCtor.Database.Entities;
using CryTraCtor.Business.Mappers.MapperBase;

namespace CryTraCtor.Business.Mappers;

public class DnsPacketModelMapper : ModelMapperBase<DnsPacketEntity, DnsPacketModel, DnsPacketModel>
{
    public override DnsPacketEntity MapToEntity(DnsPacketModel model)
        => new()
        {
            Id = model.Id,
            Timestamp = model.Timestamp,
            TransactionId = model.TransactionId,
            QueryName = model.QueryName,
            QueryType = model.QueryType,
            IsQuery = model.IsQuery,
            ResponseAddresses = model.ResponseAddresses,
            FileAnalysisId = model.FileAnalysisId
        };

    public override DnsPacketModel MapToListModel(DnsPacketEntity entity)
        => new()
        {
            Id = entity.Id,
            Timestamp = entity.Timestamp,
            TransactionId = entity.TransactionId,
            QueryName = entity.QueryName,
            QueryType = entity.QueryType,
            IsQuery = entity.IsQuery,
            ResponseAddresses = entity.ResponseAddresses,
            FileAnalysisId = entity.FileAnalysisId
        };

    public override DnsPacketModel MapToDetailModel(DnsPacketEntity entity)
        => new()
        {
            Id = entity.Id,
            Timestamp = entity.Timestamp,
            TransactionId = entity.TransactionId,
            QueryName = entity.QueryName,
            QueryType = entity.QueryType,
            IsQuery = entity.IsQuery,
            ResponseAddresses = entity.ResponseAddresses,
            FileAnalysisId = entity.FileAnalysisId
        };
}
