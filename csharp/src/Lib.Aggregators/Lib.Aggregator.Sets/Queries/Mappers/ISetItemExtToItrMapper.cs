using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Sets;

namespace Lib.Aggregator.Sets.Queries.Mappers;

/// <summary>
/// Maps ScryfallSetItemExtEntity to ISetItemItrEntity.
/// </summary>
internal interface ISetItemExtToItrMapper : ICreateMapper<ScryfallSetItemExtEntity, ISetItemItrEntity>;
