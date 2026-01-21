using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserCards;

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
