using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserSetCards.Apis;

public interface IUserSetCardsCommandDomainService
{
    Task<IOperationResponse<IUserSetCardOufEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardItrEntity entity);
    Task<IOperationResponse<IUserSetCardOufEntity>> AddCardToSetAsync(IAddCardToSetItrEntity entity);
}
