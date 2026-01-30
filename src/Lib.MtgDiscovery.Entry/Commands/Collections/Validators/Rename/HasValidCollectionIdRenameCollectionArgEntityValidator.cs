using System;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Rename;

internal sealed class HasValidCollectionIdRenameCollectionArgEntityValidator : OperationResponseValidator<IRenameCollectionArgEntity, IRenameCollectionItrEntity>
{
    public HasValidCollectionIdRenameCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IRenameCollectionArgEntity>
    {
        public Task<bool> IsValid(IRenameCollectionArgEntity arg) =>
            Task.FromResult(arg.CollectionId.IzNotNullOrWhiteSpace() && Guid.TryParse(arg.CollectionId, out _));
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Collection ID must be a valid GUID";
    }
}
