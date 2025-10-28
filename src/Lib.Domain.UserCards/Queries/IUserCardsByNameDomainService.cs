using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Marker interface for retrieving all user cards with a specific card name.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IUserCardsByNameDomainService
    : IOperationResponseService<IUserCardsNameItrEntity, IEnumerable<IUserCardOufEntity>>;
