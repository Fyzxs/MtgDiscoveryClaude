using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Sets;

internal sealed class SetIdsArgEntityValidatorContainer : ValidatorActionContainer<ISetIdsArgEntity, IOperationResponse<ISetItemCollectionOufEntity>>, ISetIdsArgEntityValidator
{
    public SetIdsArgEntityValidatorContainer() : base([
            new IsNotNullSetIdsArgEntityValidator(),
            new IdsNotNullSetIdsArgEntityValidator(),
            new HasIdsSetIdsArgEntityValidator(),
            new ValidSetIdsArgEntityValidator(),
        ])
    { }
}
