using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Users;

internal sealed class AuthUserArgEntityValidatorContainer : ValidatorActionContainer<IAuthUserArgEntity, IOperationResponse<IUserRegistrationItrEntity>>, IAuthUserArgEntityValidator
{
    public AuthUserArgEntityValidatorContainer() : base([
            new IsNotNullAuthUserArgEntityValidator(),
            new HasValidUserIdAuthUserArgEntityValidator(),
            new HasValidSourceIdAuthUserArgEntityValidator(),
            new HasValidDisplayNameAuthUserArgEntityValidator(),
        ])
    { }
}
