using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class CollectedItemSpecialValidator : OperationResponseValidator<IAddUserCardArgEntity, IUserCardOufEntity>
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

    public sealed class Validator : IValidator<IAddUserCardArgEntity>
    {

        public Task<bool> IsValid(IAddUserCardArgEntity arg)
        {
            string special = arg.UserCardDetails.Special;
            return Task.FromResult(s_validSpecials.Contains(special));
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => $"Special must be one of: {string.Join(", ", s_validSpecials)}";
    }
}
