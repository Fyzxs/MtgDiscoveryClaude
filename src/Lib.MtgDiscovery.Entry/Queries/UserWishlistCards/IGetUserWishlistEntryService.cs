using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.UserWishlistCards;

internal interface IGetUserWishlistEntryService //TODO: Implement the interface other services implement
{
    Task<IOperationResponse<List<CardItemOutEntity>>> Execute(IGetUserWishlistArgsEntity args);
}
