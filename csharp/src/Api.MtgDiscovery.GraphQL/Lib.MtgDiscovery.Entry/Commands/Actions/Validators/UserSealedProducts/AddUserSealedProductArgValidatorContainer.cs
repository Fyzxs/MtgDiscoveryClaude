using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Validators.UserSealedProducts;

internal sealed class AddUserSealedProductArgValidatorContainer : ValidatorActionContainer<IAddUserSealedProductArgEntity, IOperationResponse<IUserSealedProductOufEntity>>, IAddUserSealedProductArgValidator
{
    public AddUserSealedProductArgValidatorContainer() : base([
            new HasValidUserIdAddUserSealedProductArgValidator(),
            new HasValidProductUuidAddUserSealedProductArgValidator(),
            new HasValidSetIdAddUserSealedProductArgValidator(),
            new HasValidCountDeltaAddUserSealedProductArgValidator(),
        ])
    { }
}
