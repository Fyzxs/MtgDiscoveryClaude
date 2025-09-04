using Lib.MtgDiscovery.Entry.Queries.Validators.Components;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators;

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