using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.ArtistNameTrigrams;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Artists;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal interface IArtistNameTrigramDataExtToItrEntityMapper : ICreateMapper<ArtistNameTrigramDataExtEntity, IArtistSearchResultItrEntity>;
