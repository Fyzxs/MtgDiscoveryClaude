using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal interface IArtistSearchExtToItrMapper : ICreateMapper<IEnumerable<ArtistNameTrigramDataExtEntity>, IArtistSearchResultCollectionOufEntity>;
