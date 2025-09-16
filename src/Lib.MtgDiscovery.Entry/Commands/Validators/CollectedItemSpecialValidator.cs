using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class CollectedItemSpecialValidator : OperationResponseValidator<IAddCardToCollectionArgEntity, IUserCardCollectionItrEntity>
{
    private static readonly HashSet<string> s_validSpecials = new(System.StringComparer.OrdinalIgnoreCase)
    {
        "none",
        "altered",
        "signed",
        "proof"
    };

    public CollectedItemSpecialValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgEntity>
    {

        public Task<bool> IsValid(IAddCardToCollectionArgEntity arg)
        {
            string special = arg.CollectedItem.Special;
            return Task.FromResult(s_validSpecials.Contains(special));
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => $"Special must be one of: {string.Join(", ", s_validSpecials)}";
    }
}
