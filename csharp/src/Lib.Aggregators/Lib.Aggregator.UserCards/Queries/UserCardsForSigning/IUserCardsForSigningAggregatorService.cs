using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserCards.Queries.UserCardsForSigning;

internal interface IUserCardsForSigningAggregatorService
{
    Task<IOperationResponse<ISigningResultOufEntity>> Execute(
        IUserCardsForSigningItrEntity input,
        CancellationToken cancellationToken);
}
