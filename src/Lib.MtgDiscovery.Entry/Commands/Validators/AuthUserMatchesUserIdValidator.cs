using System;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class AuthUserMatchesUserIdValidator : OperationResponseValidator<IAddCardToCollectionArgsEntity, IUserCardOufEntity>
{
    public AuthUserMatchesUserIdValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgsEntity>
    {
        public Task<bool> IsValid(IAddCardToCollectionArgsEntity arg) =>
            Task.FromResult(string.Equals(arg.AuthUser.UserId, arg.AddUserCard.UserId, StringComparison.OrdinalIgnoreCase));
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Authenticated user does not match the provided user ID";
    }
}
