using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Outs.UserCards;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal interface IUserCardCollectionItrToOutMapper : ICreateMapper<IUserCardCollectionItrEntity, UserCardCollectionOutEntity>
{
    new Task<UserCardCollectionOutEntity> Map(IUserCardCollectionItrEntity source);
}