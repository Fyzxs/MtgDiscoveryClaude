using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Artists.Mappers;

internal interface IArtistNameTrigramDataExtToItrEntityMapper : ICreateMapper<ArtistNameTrigramDataExtEntity, IArtistSearchResultItrEntity>;
