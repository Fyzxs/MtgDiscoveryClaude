using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal interface IFinishCountsExtToOufMapper : ICreateMapper<FinishCountsExtEntity, IFinishCountsOufEntity>
{
}