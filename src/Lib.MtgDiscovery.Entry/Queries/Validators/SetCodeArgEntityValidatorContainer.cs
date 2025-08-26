using Lib.MtgDiscovery.Entry.Queries.Validators.Components;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators;

internal sealed class SetCodeArgEntityValidatorContainer : ValidatorActionContainer<ISetCodeArgEntity, IOperationResponse<ICardItemCollectionItrEntity>>, ISetCodeArgEntityValidator
{
    public SetCodeArgEntityValidatorContainer() : base([
            new IsNotNullSetCodeArgEntityValidator(),
            new HasValidSetCodeArgEntityValidator(),
        ])
    { }
}