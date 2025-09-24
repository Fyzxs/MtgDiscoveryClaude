using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class CollectedItemFinishValidator : OperationResponseValidator<IAddCardToCollectionArgsEntity, IUserCardOufEntity>
{
    private static readonly HashSet<string> s_validFinishes = new(System.StringComparer.OrdinalIgnoreCase)
    {
        "nonfoil",
        "foil",
        "etched"
    };

    public CollectedItemFinishValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgsEntity>
    {

        public Task<bool> IsValid(IAddCardToCollectionArgsEntity arg)
        {
            if (arg.AddUserCard.UserCardDetails is null) return Task.FromResult(true); // Skip if null, handled by null validator

            return Task.FromResult(s_validFinishes.Contains(arg.AddUserCard.UserCardDetails.Finish));
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => $"Finish must be one of: {string.Join(", ", s_validFinishes)}";
    }
}
