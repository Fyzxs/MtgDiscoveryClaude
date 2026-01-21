using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Validators.UserWishlistCards;

internal sealed class HasValidUserIdAddCardToWishlistArgEntityValidator : OperationResponseValidator<IAddCardToWishlistArgsEntity, IUserWishlistCardOufEntity>
{
    public HasValidUserIdAddCardToWishlistArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToWishlistArgsEntity>
    {
        public Task<bool> IsValid(IAddCardToWishlistArgsEntity arg) => Task.FromResult(arg.AddUserWishlistCard.UserId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User ID cannot be empty";
    }
}
