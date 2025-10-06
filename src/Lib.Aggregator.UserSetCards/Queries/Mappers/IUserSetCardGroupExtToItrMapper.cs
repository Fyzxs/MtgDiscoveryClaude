using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.UserSetCards.Entities;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal interface IUserSetCardGroupExtToItrMapper
{
    Task<IUserSetCardGroupOufEntity> Map(UserSetCardGroupExtEntity groupExt);
}
