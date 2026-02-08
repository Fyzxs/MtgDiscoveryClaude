using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetItems;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Sets;

namespace Lib.Aggregator.Sets.Queries.Mappers;

/// <summary>
/// Maps ScryfallSetItemExtEntity to ISetItemOufEntity.
/// </summary>
internal interface ISetItemExtToOufMapper : ICreateMapper<ScryfallSetItemExtEntity, ISetItemOufEntity>;
