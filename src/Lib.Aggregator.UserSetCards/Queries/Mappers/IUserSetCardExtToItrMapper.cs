using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.UserSetCards.Entities;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal interface IUserSetCardExtToItrMapper
{
    Task<IUserSetCardOufEntity> Map(UserSetCardExtEntity userSetCardExt);
}
