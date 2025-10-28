using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Marker interface for retrieving a specific user card using point read operation.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IUserCardDomainService
    : IOperationResponseService<IUserCardItrEntity, IEnumerable<IUserCardOufEntity>>;
