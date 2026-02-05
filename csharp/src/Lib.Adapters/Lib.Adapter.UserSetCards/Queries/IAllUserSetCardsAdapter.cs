using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSetCards;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserSetCards.Queries;

internal interface IAllUserSetCardsAdapter
{
    Task<IOperationResponse<IEnumerable<UserSetCardExtEntity>>> Execute(
        IAllUserSetCardsXfrEntity userSetCards,
        CancellationToken cancellationToken);
}
