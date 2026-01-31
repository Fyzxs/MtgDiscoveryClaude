using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.UserCards;

internal sealed class UserCardsForSigningArgEntityValidatorContainer : ValidatorActionContainer<IUserCardsForSigningArgEntity, IOperationResponse<ISigningResultOufEntity>>, IUserCardsForSigningArgEntityValidator
{
    public UserCardsForSigningArgEntityValidatorContainer() : base([
            new IsNotNullUserCardsForSigningArgEntityValidator(),
            new UserIdNotNullUserCardsForSigningArgEntityValidator(),
            new HasValidUserIdUserCardsForSigningArgEntityValidator(),
            new ArtistIdsNotNullUserCardsForSigningArgEntityValidator(),
            new ArtistIdsNotEmptyUserCardsForSigningArgEntityValidator()
        ])
    { }
}
