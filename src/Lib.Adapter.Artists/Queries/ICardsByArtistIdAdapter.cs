using System.Collections.Generic;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.Artists.Queries;

/// <summary>
/// Single-method adapter for retrieving cards by artist ID.
/// </summary>
internal interface ICardsByArtistIdAdapter
    : IOperationResponseService<IArtistIdXfrEntity, IEnumerable<ScryfallArtistCardExtEntity>>;
