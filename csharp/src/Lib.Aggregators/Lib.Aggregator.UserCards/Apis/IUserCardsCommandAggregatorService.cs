using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserCards.Apis;

public interface IUserCardsCommandAggregatorService
{
    Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(IUserCardItrEntity userCard);
    Task<IOperationResponse<IUserCardOufEntity>> AddUserCardOnlyAsync(IUserCardItrEntity userCard);
}
