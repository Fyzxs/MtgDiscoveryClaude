using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.User;
using Lib.Shared.DataModels.Entities.Args;

namespace Lib.MtgDiscovery.Entry.Queries.User;

internal interface IRegisterUserEntryService : IOperationResponseService<IAuthUserArgEntity, UserRegistrationOutEntity>
{
}
