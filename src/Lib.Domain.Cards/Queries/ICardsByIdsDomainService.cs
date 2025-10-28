using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.Cards.Queries;

/// <summary>
/// Marker interface for retrieving cards by ID collection.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface ICardsByIdsDomainService
    : IOperationResponseService<ICardIdsItrEntity, ICardItemCollectionOufEntity>;
