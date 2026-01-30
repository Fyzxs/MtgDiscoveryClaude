using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.SealedProducts;

internal sealed class SealedProductsBySetCodeArgEntityValidatorContainer : ValidatorActionContainer<ISealedProductsBySetCodeArgEntity, IOperationResponse<IEnumerable<ISealedProductOufEntity>>>, ISealedProductsBySetCodeArgEntityValidator
{
    public SealedProductsBySetCodeArgEntityValidatorContainer() : base([
            new IsNotNullSealedProductsBySetCodeArgEntityValidator(),
            new HasValidSetCodeSealedProductsBySetCodeArgEntityValidator(),
        ])
    { }
}
