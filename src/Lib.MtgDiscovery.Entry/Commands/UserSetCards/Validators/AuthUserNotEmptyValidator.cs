using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Validators;

internal sealed class AuthUserNotEmptyValidator : OperationResponseValidator<IAddSetGroupToUserSetCardArgsEntity, IUserSetCardOufEntity>
{
    public AuthUserNotEmptyValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddSetGroupToUserSetCardArgsEntity>
    {
        public Task<bool> IsValid(IAddSetGroupToUserSetCardArgsEntity arg) => Task.FromResult(arg.AuthUser?.UserId.IzNotNullOrWhiteSpace() ?? false);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Auth user ID cannot be empty";
    }
}
