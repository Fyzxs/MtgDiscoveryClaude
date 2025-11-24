using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Marker interface for retrieving all user cards for a specific user within a given set.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IUserCardsBySetDomainService
    : IOperationResponseService<IUserCardsSetItrEntity, IEnumerable<IUserCardOufEntity>>;
