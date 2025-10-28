using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.Artists.Queries;

/// <summary>
/// Marker interface for retrieving cards by artist ID.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface ICardsByArtistDomainService
    : IOperationResponseService<IArtistIdItrEntity, ICardItemCollectionOufEntity>;
