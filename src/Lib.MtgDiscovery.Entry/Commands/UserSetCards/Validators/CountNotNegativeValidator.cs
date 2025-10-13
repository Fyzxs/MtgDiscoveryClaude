using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Validators;

internal sealed class CountNotNegativeValidator : OperationResponseValidator<IAddSetGroupToUserSetCardArgsEntity, IUserSetCardOufEntity>
{
    public CountNotNegativeValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddSetGroupToUserSetCardArgsEntity>
    {
        public Task<bool> IsValid(IAddSetGroupToUserSetCardArgsEntity arg) => Task.FromResult(arg.AddSetGroupToUserSetCard != null && 0 <= arg.AddSetGroupToUserSetCard.Count);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Count cannot be negative";
    }
}
