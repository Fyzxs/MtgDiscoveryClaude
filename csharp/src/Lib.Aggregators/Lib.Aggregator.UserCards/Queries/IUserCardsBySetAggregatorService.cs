using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserCards.Queries;

internal interface IUserCardsBySetAggregatorService
{
    Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> Execute(
        IUserCardsSetItrEntity input,
        CancellationToken cancellationToken);
}
