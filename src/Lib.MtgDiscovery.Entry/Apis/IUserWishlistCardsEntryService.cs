using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface IUserWishlistCardsEntryService
{
    Task<IOperationResponse<List<CardItemOutEntity>>> AddCardToWishlistAsync(IAddCardToWishlistArgsEntity args);
    Task<IOperationResponse<List<CardItemOutEntity>>> GetUserWishlistAsync(IGetUserWishlistArgsEntity args);
}
