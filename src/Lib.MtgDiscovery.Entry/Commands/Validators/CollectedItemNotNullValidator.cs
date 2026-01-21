using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class CollectedItemNotNullValidator : OperationResponseValidator<IAddUserCardArgEntity, IUserCardOufEntity>
{
    public CollectedItemNotNullValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddUserCardArgEntity>
    {
        public Task<bool> IsValid(IAddUserCardArgEntity arg)
        {
            return Task.FromResult(arg.UserCardDetails is not null);
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Collected item cannot be null";
    }
}
