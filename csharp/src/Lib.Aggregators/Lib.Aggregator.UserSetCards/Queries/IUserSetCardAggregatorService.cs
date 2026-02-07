using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserSetCards.Queries;

internal interface IUserSetCardAggregatorService
{
    Task<IOperationResponse<IUserSetCardOufEntity>> Execute(
        IUserSetCardItrEntity input,
        CancellationToken cancellationToken);
}
