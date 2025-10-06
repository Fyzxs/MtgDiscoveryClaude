using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Outs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.UserSetCards;

internal interface IUserSetCardEntryService
{
    Task<IOperationResponse<UserSetCardOutEntity>> Execute(IUserSetCardArgEntity userSetCardArgs);
}
