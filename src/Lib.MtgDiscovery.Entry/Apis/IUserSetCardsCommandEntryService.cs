using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface IUserSetCardsCommandEntryService
{
    Task<IOperationResponse<UserSetCardOutEntity>> AddSetGroupToUserSetCardAsync(IAuthUserArgEntity authUser, IAddSetGroupToUserSetCardArgEntity argEntity);
}
