using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Delete;

internal sealed class DeleteCollectionArgEntityValidatorContainer : ValidatorActionContainer<IDeleteCollectionArgEntity, IOperationResponse<IDeleteCollectionItrEntity>>, IDeleteCollectionArgEntityValidator
{
    public DeleteCollectionArgEntityValidatorContainer() : base([
            new IsNotNullDeleteCollectionArgEntityValidator(),
            new HasValidCollectionIdDeleteCollectionArgEntityValidator(),
        ])
    { }
}
