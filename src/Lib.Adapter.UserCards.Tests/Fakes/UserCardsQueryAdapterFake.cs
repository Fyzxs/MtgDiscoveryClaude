using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserCards.Tests.Fakes;

internal sealed class UserCardsQueryAdapterFake : IUserCardsQueryAdapter
{
    public IOperationResponse<IEnumerable<UserCardExtEntity>> UserCardsBySetAsyncResult { get; init; }
    public int UserCardsBySetAsyncInvokeCount { get; private set; }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsBySetAsync(IUserCardsSetXfrEntity userCardsSet)
    {
        UserCardsBySetAsyncInvokeCount++;
        return await Task.FromResult(UserCardsBySetAsyncResult).ConfigureAwait(false);
    }
}