using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class AddCardToCollectionArgEntityValidatorContainer : ValidatorActionContainer<IAddCardToCollectionArgsEntity, IOperationResponse<IUserCardOufEntity>>, IAddCardToCollectionArgEntityValidator
{
    public AddCardToCollectionArgEntityValidatorContainer() : base([
            new HasValidCardIdAddCardToCollectionArgEntityValidator(),
            new HasValidSetIdAddCardToCollectionArgEntityValidator(),
            new HasValidUserIdAddCardToCollectionArgEntityValidator(),
            new AuthUserMatchesUserIdValidator(),
            new CollectedItemNotNullValidator(),
            new CollectedItemCountValidator(),
            new CollectedItemFinishValidator(),
            new CollectedItemSpecialValidator(),
        ])
    { }
}
