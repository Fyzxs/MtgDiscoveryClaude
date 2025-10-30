using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserSetCards.Queries;

internal interface IAllUserSetCardsAggregatorService
{
    Task<IOperationResponse<IEnumerable<IUserSetCardOufEntity>>> Execute(IAllUserSetCardsItrEntity userSetCards);
}
