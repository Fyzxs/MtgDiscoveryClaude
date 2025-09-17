using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class CollectedItemFinishValidator : OperationResponseValidator<IUserCardArgEntity, IUserCardItrEntity>
{
    private static readonly HashSet<string> s_validFinishes = new(System.StringComparer.OrdinalIgnoreCase)
    {
        "nonfoil",
        "foil",
        "etched"
    };

    public CollectedItemFinishValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardArgEntity>
    {

        public Task<bool> IsValid(IUserCardArgEntity arg)
        {
            if (arg.UserCardDetails is null) return Task.FromResult(true); // Skip if null, handled by null validator

            return Task.FromResult(s_validFinishes.Contains(arg.UserCardDetails.Finish));
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => $"Finish must be one of: {string.Join(", ", s_validFinishes)}";
    }
}
