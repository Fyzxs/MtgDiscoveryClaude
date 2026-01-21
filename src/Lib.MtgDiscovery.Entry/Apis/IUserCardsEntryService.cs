using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface IUserCardsEntryService
{
    Task<IOperationResponse<IUserCardItrEntity>> AddCardToCollectionAsync(IAuthUserArgEntity authUser, IUserCardArgEntity args);
}
