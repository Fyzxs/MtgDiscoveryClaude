using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Rename;

internal sealed class IsNotNullRenameCollectionArgEntityValidator : OperationResponseValidator<IRenameCollectionArgEntity, IRenameCollectionItrEntity>
{
    public IsNotNullRenameCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IRenameCollectionArgEntity>
    {
        public Task<bool> IsValid(IRenameCollectionArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Rename collection argument cannot be null";
    }
}
