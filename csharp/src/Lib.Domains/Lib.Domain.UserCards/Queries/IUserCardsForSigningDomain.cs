using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.UserCards.Queries;

internal interface IUserCardsForSigningDomain
    : IOperationResponseService<IUserCardsForSigningItrEntity, ISigningResultOufEntity>;
