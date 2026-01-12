using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Validators.UserWishlistCards;

internal sealed class AddCardToWishlistArgEntityValidatorContainer : ValidatorActionContainer<IAddCardToWishlistArgsEntity, IOperationResponse<IUserWishlistCardOufEntity>>, IAddCardToWishlistArgEntityValidator
{
    public AddCardToWishlistArgEntityValidatorContainer() : base([
            new HasValidCardIdAddCardToWishlistArgEntityValidator(),
            new HasValidSetIdAddCardToWishlistArgEntityValidator(),
            new HasValidUserIdAddCardToWishlistArgEntityValidator(),
            new AuthUserMatchesUserIdWishlistValidator(),
            new WishlistItemNotNullValidator(),
            new WishlistItemCountValidator(),
            new WishlistItemFinishValidator(),
            new WishlistItemSpecialValidator(),
        ])
    { }
}
