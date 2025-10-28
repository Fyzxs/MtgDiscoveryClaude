using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Marker interface for retrieving multiple user cards using batch point read operations.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IUserCardsByIdsDomainService
    : IOperationResponseService<IUserCardsByIdsItrEntity, IEnumerable<IUserCardOufEntity>>;
