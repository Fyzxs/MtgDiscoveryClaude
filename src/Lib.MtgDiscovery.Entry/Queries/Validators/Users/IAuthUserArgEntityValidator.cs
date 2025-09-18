using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Users;

internal interface IAuthUserArgEntityValidator : IValidatorAction<IAuthUserArgEntity, IOperationResponse<IUserRegistrationItrEntity>>
{
}
