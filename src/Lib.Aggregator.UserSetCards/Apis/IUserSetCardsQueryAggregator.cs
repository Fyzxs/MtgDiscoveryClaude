using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserSetCards.Apis;

public interface IUserSetCardsQueryAggregator
{
    Task<IOperationResponse<IUserSetCardOufEntity>> UserSetCardByUserAndSetAsync(IUserSetCardItrEntity userSetCard);

    Task<IOperationResponse<IEnumerable<IUserSetCardOufEntity>>> AllUserSetCardsAsync(IAllUserSetCardsItrEntity userSetCards);
}
