using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Artists.Queries;

/// <summary>
/// Single-method adapter for retrieving cards by artist ID.
/// </summary>
internal interface ICardsByArtistIdAdapter
{
    Task<IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>> Execute(IArtistIdXfrEntity input, CancellationToken cancellationToken);
}
