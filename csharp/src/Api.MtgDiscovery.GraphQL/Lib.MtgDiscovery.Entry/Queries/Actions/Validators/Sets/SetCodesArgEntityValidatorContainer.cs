using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Oufs.Sets;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.Sets;

internal sealed class SetCodesArgEntityValidatorContainer : ValidatorActionContainer<ISetCodesArgEntity, IOperationResponse<ISetItemCollectionOufEntity>>, ISetCodesArgEntityValidator
{
    public SetCodesArgEntityValidatorContainer() : base([
            new IsNotNullSetCodesArgEntityValidator(),
            new CodesNotNullSetCodesArgEntityValidator(),
            new HasCodesSetCodesArgEntityValidator(),
            new ValidSetCodesArgEntityValidator(),
        ])
    { }
}
