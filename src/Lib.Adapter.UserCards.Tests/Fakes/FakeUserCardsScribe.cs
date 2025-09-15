using System.Net;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.UserCards.Tests.Fakes;

internal sealed class FakeUserCardsScribe : ICosmosScribe
{
    public int UpsertAsyncCallCount { get; private set; }

    public async Task<OpResponse<T>> UpsertAsync<T>(T item)
    {
        UpsertAsyncCallCount++;
        await Task.CompletedTask.ConfigureAwait(false);

        // Return success response with the same item
        return new FakeOpResponse<T>(item, HttpStatusCode.OK);
    }
}

internal sealed class FakeOpResponse<T> : OpResponse<T>
{
    public FakeOpResponse(T value, HttpStatusCode statusCode)
    {
        Value = value;
        StatusCode = statusCode;
    }

    public override T Value { get; }
    public override HttpStatusCode StatusCode { get; }
}
