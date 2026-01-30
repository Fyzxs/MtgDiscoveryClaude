using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.UserCards;

internal sealed class UserCardsByIdsArgEntityValidatorContainer : ValidatorActionContainer<IUserCardsByIdsArgEntity, IOperationResponse<IEnumerable<IUserCardOufEntity>>>, IUserCardsByIdsArgEntityValidator
{
    public UserCardsByIdsArgEntityValidatorContainer() : base([
            new IsNotNullUserCardsByIdsArgEntityValidator(),
            new UserIdNotNullUserCardsByIdsArgEntityValidator(),
            new HasValidUserIdUserCardsByIdsArgEntityValidator(),
            new CardIdsNotNullUserCardsByIdsArgEntityValidator(),
            new CardIdsNotEmptyUserCardsByIdsArgEntityValidator(),
            new ValidCardIdsUserCardsByIdsArgEntityValidator()
        ])
    { }
}
