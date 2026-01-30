using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Rename;

internal sealed class NameNotTooLongRenameCollectionArgEntityValidator : OperationResponseValidator<IRenameCollectionArgEntity, IRenameCollectionItrEntity>
{
    public NameNotTooLongRenameCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IRenameCollectionArgEntity>
    {
        public Task<bool> IsValid(IRenameCollectionArgEntity arg) => Task.FromResult(arg.Name.Length < 101);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Collection name cannot exceed 100 characters";
    }
}
