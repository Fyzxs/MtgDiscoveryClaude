using Lib.MtgDiscovery.Entry.Entities.Outs.User;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.Invocation.Services;

namespace Lib.MtgDiscovery.Entry.Queries.User;

internal interface IRegisterUserEntryService : IOperationResponseService<IAuthUserArgEntity, UserSyncOutEntity>
{
}
