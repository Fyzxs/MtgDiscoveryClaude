using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class CollectedItemFinishValidator : OperationResponseValidator<IAddCardToCollectionArgEntity, IUserCardCollectionItrEntity>
{
    private static readonly HashSet<string> s_validFinishes = new(System.StringComparer.OrdinalIgnoreCase)
    {
        "nonfoil",
        "foil",
        "etched"
    };

    public CollectedItemFinishValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgEntity>
    {

        public Task<bool> IsValid(IAddCardToCollectionArgEntity arg)
        {
            if (arg.CollectedItem is null) return Task.FromResult(true); // Skip if null, handled by null validator

            return Task.FromResult(s_validFinishes.Contains(arg.CollectedItem.Finish));
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => $"Finish must be one of: {string.Join(", ", s_validFinishes)}";
    }
}
