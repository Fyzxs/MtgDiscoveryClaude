using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards;

internal interface IAddSetGroupToUserSetCardEntryService
{
    Task<IOperationResponse<UserSetCardOutEntity>> Execute(IAddSetGroupToUserSetCardArgsEntity args);
}