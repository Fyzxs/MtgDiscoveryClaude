using System.Collections.Generic;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.Artists.Queries;

/// <summary>
/// Single-method adapter for searching artists using trigram matching.
/// </summary>
internal interface ISearchArtistsAdapter
    : IOperationResponseService<IArtistSearchTermXfrEntity, IEnumerable<ArtistNameTrigramDataExtEntity>>;
