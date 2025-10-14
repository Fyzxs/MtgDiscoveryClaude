using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserCards;

internal sealed class UserCardArgEntityValidatorContainer : ValidatorActionContainer<IUserCardArgEntity, IOperationResponse<IEnumerable<IUserCardOufEntity>>>, IUserCardArgEntityValidator
{
    public UserCardArgEntityValidatorContainer() : base([
            new IsNotNullUserCardArgEntityValidator(),
            new UserIdNotNullUserCardArgEntityValidator(),
            new HasValidUserIdUserCardArgEntityValidator(),
            new CardIdNotNullUserCardArgEntityValidator(),
            new HasValidCardIdUserCardArgEntityValidator()
        ])
    { }
}
