using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.UserCards.Tests.Fakes;

public sealed class UserCardsGopherFake : ICosmosGopher
{
    public int ReadAsyncCallCount { get; private set; }
    public ReadPointItem ReadAsyncReadPointItemInput { get; private set; } = default!;
    public bool ShouldReturnExistingRecord { get; init; }
    public UserCardExtEntity ExistingRecord { get; init; } = default!;

    public Task<OpResponse<T>> ReadAsync<T>(ReadPointItem item, CancellationToken cancellationToken = default)
    {
        ReadAsyncCallCount++;
        ReadAsyncReadPointItemInput = item;

        if (ShouldReturnExistingRecord && ExistingRecord is T typedValue)
        {
            return Task.FromResult<OpResponse<T>>(new OpResponseFake<T>(typedValue, HttpStatusCode.OK));
        }

        return Task.FromResult<OpResponse<T>>(new OpResponseFake<T>(default!, HttpStatusCode.NotFound));
    }
}
