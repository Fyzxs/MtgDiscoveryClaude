using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class CollectedItemCountValidator : OperationResponseValidator<IUserCardArgEntity, IUserCardItrEntity>
{
    public CollectedItemCountValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserCardArgEntity>
    {
        public Task<bool> IsValid(IUserCardArgEntity arg)
        {
            return Task.FromResult(arg.UserCardDetails?.Count != 0);
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Count cannot be 0";
    }
}