using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Transfer;

internal sealed class IsNotNullTransferCollectionOwnershipArgEntityValidator : OperationResponseValidator<ITransferCollectionOwnershipArgEntity, ITransferCollectionOwnershipItrEntity>
{
    public IsNotNullTransferCollectionOwnershipArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ITransferCollectionOwnershipArgEntity>
    {
        public Task<bool> IsValid(ITransferCollectionOwnershipArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Transfer collection ownership argument cannot be null";
    }
}
