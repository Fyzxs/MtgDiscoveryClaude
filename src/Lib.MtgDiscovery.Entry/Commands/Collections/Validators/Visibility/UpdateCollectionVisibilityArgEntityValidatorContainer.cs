using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Visibility;

internal sealed class UpdateCollectionVisibilityArgEntityValidatorContainer : ValidatorActionContainer<IUpdateCollectionVisibilityArgEntity, IOperationResponse<IUpdateCollectionVisibilityItrEntity>>, IUpdateCollectionVisibilityArgEntityValidator
{
    public UpdateCollectionVisibilityArgEntityValidatorContainer() : base([
            new IsNotNullUpdateCollectionVisibilityArgEntityValidator(),
            new HasValidCollectionIdUpdateCollectionVisibilityArgEntityValidator(),
            new HasValidVisibilityUpdateCollectionVisibilityArgEntityValidator(),
        ])
    { }
}
