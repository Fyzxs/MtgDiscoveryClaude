using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.UserCards;

internal sealed class UserCardsSetArgEntityValidatorContainer : ValidatorActionContainer<IUserCardsBySetArgEntity, IOperationResponse<IEnumerable<IUserCardOufEntity>>>, IUserCardsSetArgEntityValidator
{
    public UserCardsSetArgEntityValidatorContainer() : base([
            new IsNotNullUserCardsSetArgEntityValidator(),
            new SetIdNotNullUserCardsSetArgEntityValidator(),
            new HasValidSetIdUserCardsSetArgEntityValidator(),
            new UserIdNotNullUserCardsSetArgEntityValidator(),
            new HasValidUserIdUserCardsSetArgEntityValidator()
        ])
    { }
}
