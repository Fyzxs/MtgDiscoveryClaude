using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class CollectedItemFinishValidator : OperationResponseValidator<IAddCardToCollectionArgEntity, IUserCardCollectionItrEntity>
{
    public CollectedItemFinishValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgEntity>
    {
        internal static readonly HashSet<string> ValidFinishes = new(System.StringComparer.OrdinalIgnoreCase)
        {
            "nonfoil",
            "foil",
            "etched"
        };

        public Task<bool> IsValid(IAddCardToCollectionArgEntity arg)
        {
            if (arg.CollectedItem is null) return Task.FromResult(true); // Skip if null, handled by null validator

            return Task.FromResult(ValidFinishes.Contains(arg.CollectedItem.Finish));
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => $"Finish must be one of: {string.Join(", ", Validator.ValidFinishes)}";
    }
}