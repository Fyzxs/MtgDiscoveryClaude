using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface IUserSetCardsCommandEntryService
{
    Task<IOperationResponse<UserSetCardOutEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardArgsEntity args, CancellationToken cancellationToken);
    Task<IOperationResponse<UserSetCardOutEntity>> AddCardToSetAsync(IAddCardToSetArgsEntity args, CancellationToken cancellationToken);
}
