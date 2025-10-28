using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.Artists.Queries;

/// <summary>
/// Marker interface for artist search operation.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IArtistSearchDomainService
    : IOperationResponseService<IArtistSearchTermItrEntity, IArtistSearchResultCollectionOufEntity>;
