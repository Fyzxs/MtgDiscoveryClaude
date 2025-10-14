using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserSetCards;

internal sealed class UserSetCardArgEntityValidatorContainer : ValidatorActionContainer<IUserSetCardArgEntity, IOperationResponse<IUserSetCardOufEntity>>, IUserSetCardArgEntityValidator
{
    public UserSetCardArgEntityValidatorContainer() : base([
            new IsNotNullUserSetCardArgEntityValidator(),
            new UserIdNotNullUserSetCardArgEntityValidator(),
            new HasValidUserIdUserSetCardArgEntityValidator(),
            new SetIdNotNullUserSetCardArgEntityValidator(),
            new HasValidSetIdUserSetCardArgEntityValidator()
        ])
    { }
}
