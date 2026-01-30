using System.Net;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.UserCards.Tests.Fakes;

internal sealed class UserCardsScribeFake : ICosmosScribe
{
    public int UpsertAsyncCallCount { get; private set; }
    public bool ShouldReturnFailure { get; init; }
    public HttpStatusCode FailureStatusCode { get; init; } = HttpStatusCode.InternalServerError;

    public Task<OpResponse<T>> UpsertAsync<T>(T item)
    {
        UpsertAsyncCallCount++;

        if (ShouldReturnFailure)
        {
            return Task.FromResult<OpResponse<T>>(new OpResponseFake<T>(default!, FailureStatusCode));
        }

        return Task.FromResult<OpResponse<T>>(new OpResponseFake<T>(item, HttpStatusCode.OK));
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
