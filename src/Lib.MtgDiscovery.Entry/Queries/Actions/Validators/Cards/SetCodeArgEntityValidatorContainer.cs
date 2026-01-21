using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.Cards;

internal sealed class SetCodeArgEntityValidatorContainer : ValidatorActionContainer<ISetCodeArgEntity, IOperationResponse<ICardItemCollectionOufEntity>>, ISetCodeArgEntityValidator
{
    public SetCodeArgEntityValidatorContainer() : base([
            new IsNotNullSetCodeArgEntityValidator(),
            new HasValidSetCodeArgEntityValidator(),
        ])
    { }
}
