using System;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Validators;

internal sealed class AuthUserMatchesUserIdSetCardValidator : OperationResponseValidator<IAddSetGroupToUserSetCardArgsEntity, IUserSetCardOufEntity>
{
    public AuthUserMatchesUserIdSetCardValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddSetGroupToUserSetCardArgsEntity>
    {
        public Task<bool> IsValid(IAddSetGroupToUserSetCardArgsEntity arg) =>
            Task.FromResult(string.Equals(arg.AuthUser.UserId, arg.AddSetGroupToUserSetCard.UserId, StringComparison.OrdinalIgnoreCase));
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Authenticated user does not match the provided user ID";
    }
}
