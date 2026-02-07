using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards;

/// <summary>
/// Entry service for adding a card to a user's set collection.
/// Used by migration tools for direct UserSetCards updates.
/// </summary>
internal interface IAddCardToSetEntryService
{
    Task<IOperationResponse<UserSetCardOutEntity>> Execute(IAddCardToSetArgsEntity argsEntity, CancellationToken cancellationToken);
}
