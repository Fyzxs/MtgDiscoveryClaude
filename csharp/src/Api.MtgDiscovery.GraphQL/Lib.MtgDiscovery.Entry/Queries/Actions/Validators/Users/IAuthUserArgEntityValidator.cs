using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Itrs.User;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.Users;

internal interface IAuthUserArgEntityValidator : IValidatorAction<IAuthUserArgEntity, IOperationResponse<IUserRegistrationItrEntity>>
{
}
