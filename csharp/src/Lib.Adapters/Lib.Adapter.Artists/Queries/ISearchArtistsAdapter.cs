using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Artists.Queries;

/// <summary>
/// Single-method adapter for searching artists using trigram matching.
/// </summary>
internal interface ISearchArtistsAdapter
{
    Task<IOperationResponse<IEnumerable<ArtistNameTrigramDataExtEntity>>> Execute(IArtistSearchTermXfrEntity input, CancellationToken cancellationToken);
}
