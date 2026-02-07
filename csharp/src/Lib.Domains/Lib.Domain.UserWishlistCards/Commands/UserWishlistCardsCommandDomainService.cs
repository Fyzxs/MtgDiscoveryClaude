using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.UserWishlistCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserWishlistCards.Commands;

internal sealed class UserWishlistCardsCommandDomainService : IUserWishlistCardsCommandDomainService
{
    private readonly IAddUserWishlistCardDomainService _addUserWishlistCardService;
    private readonly IRemoveUserWishlistCardDomainService _removeUserWishlistCardService;

    public UserWishlistCardsCommandDomainService(ILogger logger) : this(
        new AddUserWishlistCardDomainService(logger),
        new RemoveUserWishlistCardDomainService(logger))
    { }

    private UserWishlistCardsCommandDomainService(
        IAddUserWishlistCardDomainService addUserWishlistCardService,
        IRemoveUserWishlistCardDomainService removeUserWishlistCardService)
    {
        _addUserWishlistCardService = addUserWishlistCardService;
        _removeUserWishlistCardService = removeUserWishlistCardService;
    }

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> AddUserWishlistCardAsync(IUserWishlistCardItrEntity userWishlistCard, CancellationToken cancellationToken) => await _addUserWishlistCardService.Execute(userWishlistCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> RemoveUserWishlistCardAsync(IUserWishlistCardItrEntity userWishlistCard, CancellationToken cancellationToken) => await _removeUserWishlistCardService.Execute(userWishlistCard, cancellationToken).ConfigureAwait(false);
}
