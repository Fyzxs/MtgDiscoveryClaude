using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Rename;

internal sealed class RenameCollectionArgEntityValidatorContainer : ValidatorActionContainer<IRenameCollectionArgEntity, IOperationResponse<IRenameCollectionItrEntity>>, IRenameCollectionArgEntityValidator
{
    public RenameCollectionArgEntityValidatorContainer() : base([
            new IsNotNullRenameCollectionArgEntityValidator(),
            new HasValidCollectionIdRenameCollectionArgEntityValidator(),
            new HasValidNameRenameCollectionArgEntityValidator(),
            new NameNotTooLongRenameCollectionArgEntityValidator(),
            new NameNotReservedRenameCollectionArgEntityValidator(),
            new NameAlphanumericRenameCollectionArgEntityValidator(),
        ])
    { }
}
