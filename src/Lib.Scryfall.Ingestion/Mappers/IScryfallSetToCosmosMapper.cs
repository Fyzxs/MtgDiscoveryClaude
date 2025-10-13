using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Scryfall.Shared.Apis.Models;
using Lib.Shared.Abstractions.Mappers;

namespace Lib.Scryfall.Ingestion.Mappers;

internal interface IScryfallSetToCosmosMapper : ICreateMapper<IScryfallSet, ScryfallSetItemExtEntity>
{
}
