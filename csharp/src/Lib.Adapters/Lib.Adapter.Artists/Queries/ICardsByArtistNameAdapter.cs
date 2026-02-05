using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Artists.Queries;

/// <summary>
/// Single-method adapter for retrieving cards by artist name with disambiguation logic.
/// </summary>
internal interface ICardsByArtistNameAdapter
{
    Task<IOperationResponse<IEnumerable<ScryfallArtistCardExtEntity>>> Execute(IArtistNameXfrEntity input, CancellationToken cancellationToken);
}
