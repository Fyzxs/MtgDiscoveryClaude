using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Marker interface for retrieving user cards by multiple artists for convention signing planning.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface IUserCardsForSigningDomainService
    : IOperationResponseService<IUserCardsForSigningItrEntity, ISigningResultOufEntity>;
