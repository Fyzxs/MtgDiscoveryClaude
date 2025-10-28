using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.Artists.Queries;

/// <summary>
/// Marker interface for retrieving cards by artist name.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface ICardsByArtistNameDomainService
    : IOperationResponseService<IArtistNameItrEntity, ICardItemCollectionOufEntity>;
