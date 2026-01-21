using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class CollectedItemSpecialValidator : OperationResponseValidator<IAddCardToCollectionArgEntity, IUserCardCollectionItrEntity>
{
    public CollectedItemSpecialValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgEntity>
    {
        internal static readonly HashSet<string> ValidSpecials = new(System.StringComparer.OrdinalIgnoreCase)
        {
            "none",
            "altered",
            "signed",
            "proof"
        };

        public Task<bool> IsValid(IAddCardToCollectionArgEntity arg)
        {
            if (arg.CollectedItem is null) return Task.FromResult(true); // Skip if null, handled by null validator

            string special = arg.CollectedItem.Special ?? "none"; // Treat null as "none"
            return Task.FromResult(ValidSpecials.Contains(special));
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => $"Special must be one of: {string.Join(", ", Validator.ValidSpecials)}";
    }
}