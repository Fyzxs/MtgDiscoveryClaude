using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.Cards.Queries;

/// <summary>
/// Marker interface for retrieving cards by name.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface ICardsByNameDomainService
    : IOperationResponseService<ICardNameItrEntity, ICardItemCollectionOufEntity>;
