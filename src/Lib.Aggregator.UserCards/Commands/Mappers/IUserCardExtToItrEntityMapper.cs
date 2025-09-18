using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

/// <summary>
/// Maps UserCardExtEntity to IUserCardItrEntity.
/// </summary>
internal interface IUserCardExtToItrEntityMapper
{
    Task<IUserCardItrEntity> Map(UserCardExtEntity source);
}
