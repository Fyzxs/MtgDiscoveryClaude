using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal sealed class CollectedItemCountValidator : OperationResponseValidator<IAddCardToCollectionArgsEntity, IUserCardOufEntity>
{
    public CollectedItemCountValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAddCardToCollectionArgsEntity>
    {
        public Task<bool> IsValid(IAddCardToCollectionArgsEntity arg)
        {
            return Task.FromResult(arg.AddUserCard.UserCardDetails?.Count != 0);
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Count cannot be 0";
    }
}
