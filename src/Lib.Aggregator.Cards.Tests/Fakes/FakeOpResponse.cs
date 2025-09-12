using System.Net;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class FakeOpResponse<T> : OpResponse<T>
{
    private readonly T _value;
    private readonly HttpStatusCode _statusCode;

    public FakeOpResponse(T value, HttpStatusCode statusCode)
    {
        _value = value;
        _statusCode = statusCode;
    }

    public override T Value => _value;
    public override HttpStatusCode StatusCode => _statusCode;
}
