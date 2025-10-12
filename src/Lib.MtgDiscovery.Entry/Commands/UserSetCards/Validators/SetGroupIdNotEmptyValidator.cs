using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Entities;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Validators;

internal sealed class SetGroupIdNotEmptyValidator : OperationResponseValidator<IAddSetGroupToUserSetCardArgsEntity, IUserSetCardOufEntity>
{
    public SetGroupIdNotEmptyValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddSetGroupToUserSetCardArgsEntity>
    {
        public Task<bool> IsValid(IAddSetGroupToUserSetCardArgsEntity arg) => Task.FromResult(arg.AddSetGroupToUserSetCard.SetGroupId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set Group ID cannot be empty";
    }
}
