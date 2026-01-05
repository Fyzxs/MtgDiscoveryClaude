using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserCards.Queries.UserCardsForSigning;

internal interface IUserCardsForSigningAggregatorService
    : IOperationResponseService<IUserCardsForSigningItrEntity, ISigningResultOufEntity>;
