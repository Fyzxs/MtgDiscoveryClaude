using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Validators.UserWishlistCards;

internal sealed class WishlistItemSpecialValidator : OperationResponseValidator<IAddCardToWishlistArgsEntity, IUserWishlistCardOufEntity>
{
    public WishlistItemSpecialValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToWishlistArgsEntity>
    {
        public Task<bool> IsValid(IAddCardToWishlistArgsEntity arg) => Task.FromResult(arg.AddUserWishlistCard.UserWishlistCardDetails.Special.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Wishlist item special cannot be empty";
    }
}
