using System;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Rename;

internal sealed class NameNotReservedRenameCollectionArgEntityValidator : OperationResponseValidator<IRenameCollectionArgEntity, IRenameCollectionItrEntity>
{
    public NameNotReservedRenameCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IRenameCollectionArgEntity>
    {
        public Task<bool> IsValid(IRenameCollectionArgEntity arg) => Task.FromResult(string.Equals(arg.Name, "default", StringComparison.OrdinalIgnoreCase) is false);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Collection name 'default' is reserved";
    }
}
