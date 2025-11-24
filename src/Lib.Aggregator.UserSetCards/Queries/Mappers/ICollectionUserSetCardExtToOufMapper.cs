using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal interface ICollectionUserSetCardExtToOufMapper
{
    Task<IEnumerable<IUserSetCardOufEntity>> Map([NotNull] IEnumerable<UserSetCardExtEntity> source);
}