using Lib.MtgDiscovery.Entry.Queries.Validators.Components;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators;

internal sealed class CardNameArgEntityValidatorContainer : ValidatorActionContainer<ICardNameArgEntity, IOperationResponse<ICardItemCollectionItrEntity>>, ICardNameArgEntityValidator
{
    public CardNameArgEntityValidatorContainer() : base([
            new IsNotNullCardNameArgEntityValidator(),
            new HasValidCardNameArgEntityValidator(),
        ])
    { }
}