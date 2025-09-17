using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class CollectedItemNotNullValidator : OperationResponseValidator<IAddCardToCollectionArgEntity, IUserCardCollectionItrEntity>
{
    public CollectedItemNotNullValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgEntity>
    {
        public Task<bool> IsValid(IAddCardToCollectionArgEntity arg)
        {
            return Task.FromResult(arg.CollectedItem is not null);
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Collected item cannot be null";
    }
}
