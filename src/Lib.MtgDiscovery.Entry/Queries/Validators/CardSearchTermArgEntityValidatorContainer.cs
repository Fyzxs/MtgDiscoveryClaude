using Lib.MtgDiscovery.Entry.Queries.Validators.Components;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators;

internal sealed class CardSearchTermArgEntityValidatorContainer : ValidatorActionContainer<ICardSearchTermArgEntity, IOperationResponse<ICardNameSearchResultCollectionItrEntity>>, ICardSearchTermArgEntityValidator
{
    public CardSearchTermArgEntityValidatorContainer() : base([
            new IsNotNullCardSearchTermArgEntityValidator(),
            new HasValidSearchTermArgEntityValidator(),
            new HasMinimumLengthSearchTermArgEntityValidator(),
        ])
    { }
}