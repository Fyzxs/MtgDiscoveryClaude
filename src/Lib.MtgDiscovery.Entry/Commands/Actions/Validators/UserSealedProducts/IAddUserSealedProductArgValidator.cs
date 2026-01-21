using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Validators.UserSealedProducts;

internal interface IAddUserSealedProductArgValidator : IValidatorAction<IAddUserSealedProductArgEntity, IOperationResponse<IUserSealedProductOufEntity>>;
