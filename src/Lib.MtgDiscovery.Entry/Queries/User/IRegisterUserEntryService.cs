using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Outs.User;

namespace Lib.MtgDiscovery.Entry.Queries.User;

internal interface IRegisterUserEntryService : IOperationResponseService<IAuthUserArgEntity, UserRegistrationOutEntity>
{
}