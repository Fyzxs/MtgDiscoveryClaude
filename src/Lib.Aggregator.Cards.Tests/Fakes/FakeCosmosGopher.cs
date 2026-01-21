using System.Net;
using System.Threading.Tasks;
using Lib.Cosmos.Apis;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class FakeCosmosGopher : ICosmosGopher
{
    public OpResponse<CosmosItem> ReadAsyncResult { get; init; } = new FakeOpResponse<CosmosItem>(null!, HttpStatusCode.NotFound);
    public int ReadAsyncInvokeCount { get; private set; }
    public ReadPointItem ReadAsyncReadPointItemInput { get; private set; } = default!;

    public Task<OpResponse<T>> ReadAsync<T>(ReadPointItem readPointItem)
    {
        ReadAsyncInvokeCount++;
        ReadAsyncReadPointItemInput = readPointItem;

        if (ReadAsyncResult.Value is T typedValue)
        {
            return Task.FromResult<OpResponse<T>>(new FakeOpResponse<T>(typedValue, ReadAsyncResult.StatusCode));
        }

        if (ReadAsyncResult.Value is null)
        {
            return Task.FromResult<OpResponse<T>>(new FakeOpResponse<T>(default!, ReadAsyncResult.StatusCode));
        }

        return Task.FromResult<OpResponse<T>>(new FakeOpResponse<T>(default!, ReadAsyncResult.StatusCode));
    }
}
