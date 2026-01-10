using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Validators.UserWishlistCards;

internal interface IAddCardToWishlistArgEntityValidator : IValidatorAction<IAddCardToWishlistArgsEntity, IOperationResponse<IUserWishlistCardOufEntity>>
{
}
