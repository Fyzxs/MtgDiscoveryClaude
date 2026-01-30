using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators;

internal sealed class CreateCollectionArgEntityValidatorContainer : ValidatorActionContainer<ICreateCollectionArgEntity, IOperationResponse<ICollectionItrEntity>>, ICreateCollectionArgEntityValidator
{
    public CreateCollectionArgEntityValidatorContainer() : base([
            new IsNotNullCreateCollectionArgEntityValidator(),
            new HasValidNameCreateCollectionArgEntityValidator(),
            new NameNotTooLongCreateCollectionArgEntityValidator(),
            new NameNotReservedCreateCollectionArgEntityValidator(),
            new NameAlphanumericCreateCollectionArgEntityValidator(),
            new HasValidTypeCreateCollectionArgEntityValidator(),
            new HasValidVisibilityCreateCollectionArgEntityValidator(),
        ])
    { }
}
