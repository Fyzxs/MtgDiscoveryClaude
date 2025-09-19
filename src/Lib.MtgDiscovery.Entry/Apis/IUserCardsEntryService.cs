using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Outs.UserCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface IUserCardsEntryService
{
    Task<IOperationResponse<UserCardOutEntity>> AddCardToCollectionAsync(IAuthUserArgEntity authUser, IAddUserCardArgEntity args);
}
