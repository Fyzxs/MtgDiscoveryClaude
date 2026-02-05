using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserCards.Commands;

internal interface IAddUserCardAggregatorService
{
    Task<IOperationResponse<IUserCardOufEntity>> Execute(
        IUserCardItrEntity input,
        CancellationToken cancellationToken);
}
