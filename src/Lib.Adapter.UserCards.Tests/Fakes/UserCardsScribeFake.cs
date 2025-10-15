using System.Net;
using System.Threading.Tasks;

namespace Lib.Adapter.UserCards.Tests.Fakes;

internal sealed class UserCardsScribeFake : ICosmosScribe
{
    public int UpsertAsyncCallCount { get; private set; }
    public bool ShouldReturnFailure { get; init; }
    public HttpStatusCode FailureStatusCode { get; init; } = HttpStatusCode.InternalServerError;

    public async Task<OpResponse<T>> UpsertAsync<T>(T item)
    {
        UpsertAsyncCallCount++;
        await Task.CompletedTask.ConfigureAwait(false);

        if (ShouldReturnFailure)
        {
            return new OpResponseFake<T>(default!, FailureStatusCode);
        }

        // Return success response with the same item
        return new OpResponseFake<T>(item, HttpStatusCode.OK);
    }
}

internal sealed class OpResponseFake<T> : OpResponse<T>
{
    public OpResponseFake(T value, HttpStatusCode statusCode)
    {
        Value = value;
        StatusCode = statusCode;
    }

    public override T Value { get; }
    public override HttpStatusCode StatusCode { get; }
}
