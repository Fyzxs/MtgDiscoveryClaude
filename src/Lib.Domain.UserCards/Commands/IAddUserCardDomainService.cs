using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.UserCards.Commands;

/// <summary>
/// Marker interface for adding a user card to the collection.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IAddUserCardDomainService
    : IOperationResponseService<IUserCardItrEntity, IUserCardOufEntity>;
