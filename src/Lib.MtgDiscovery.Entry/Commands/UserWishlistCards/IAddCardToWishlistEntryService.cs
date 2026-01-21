using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.UserWishlistCards;

internal interface IAddCardToWishlistEntryService
{
    Task<IOperationResponse<List<CardItemOutEntity>>> Execute(IAddCardToWishlistArgsEntity args);
}
