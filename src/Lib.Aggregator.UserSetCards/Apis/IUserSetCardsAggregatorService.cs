using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserSetCards.Apis;

public interface IUserSetCardsAggregatorService
{
    Task<IOperationResponse<IUserSetCardOufEntity>> GetUserSetCardByUserAndSetAsync(IUserSetCardItrEntity userSetCard);
}
