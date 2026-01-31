using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.GrantAccess;

internal sealed class IsNotNullGrantCollectionAccessArgEntityValidator : OperationResponseValidator<IGrantCollectionAccessArgEntity, IGrantCollectionAccessItrEntity>
{
    public IsNotNullGrantCollectionAccessArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IGrantCollectionAccessArgEntity>
    {
        public Task<bool> IsValid(IGrantCollectionAccessArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Grant collection access argument cannot be null";
    }
}
