using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Validators.UserSealedProducts;

internal sealed class AddSealedProductToCollectionArgsValidatorContainer
    : ValidatorActionContainer<IAddSealedProductToCollectionArgsEntity, IOperationResponse<List<SealedProductOutEntity>>>,
      IAddSealedProductToCollectionArgsValidator
{
    public AddSealedProductToCollectionArgsValidatorContainer() : base([
            new HasValidAuthUserAddSealedProductToCollectionArgsValidator(),
            new HasValidProductUuidAddSealedProductToCollectionArgsValidator(),
            new HasValidSetIdAddSealedProductToCollectionArgsValidator(),
            new HasValidCountDeltaAddSealedProductToCollectionArgsValidator()
        ])
    { }
}
