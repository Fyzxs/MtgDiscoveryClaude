using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal interface IArtistNameTrigramDataExtToItrEntityMapper : ICreateMapper<ArtistNameTrigramDataExtEntity, IArtistSearchResultItrEntity>;
