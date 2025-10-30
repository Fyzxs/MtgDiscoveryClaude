using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserSetCards;

internal sealed class AllUserSetCardsArgEntityValidatorContainer : ValidatorActionContainer<IAllUserSetCardsArgEntity, IOperationResponse<IEnumerable<IUserSetCardOufEntity>>>, IAllUserSetCardsArgEntityValidator
{
    public AllUserSetCardsArgEntityValidatorContainer() : base([
            new IsNotNullAllUserSetCardsArgEntityValidator(),
            new HasValidUserIdAllUserSetCardsArgEntityValidator()
        ])
    { }
}
