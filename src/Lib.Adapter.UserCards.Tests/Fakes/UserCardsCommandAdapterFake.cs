using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserCards.Tests.Fakes;

internal sealed class UserCardsCommandAdapterFake : IUserCardsCommandAdapter
{
    public IOperationResponse<UserCardExtEntity> AddUserCardAsyncResult { get; init; }
    public int AddUserCardAsyncInvokeCount { get; private set; }

    public async Task<IOperationResponse<UserCardExtEntity>> AddUserCardAsync(IUserCardItrEntity userCard)
    {
        AddUserCardAsyncInvokeCount++;
        return await Task.FromResult(AddUserCardAsyncResult).ConfigureAwait(false);
    }
}