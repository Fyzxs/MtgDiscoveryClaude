using System.Net;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Cosmos.Operators;

internal sealed class ProvidedOpResponse<T> : OpResponse<T>
{
    public ProvidedOpResponse(T itemResponse, HttpStatusCode statusCode)
    {
        Value = itemResponse;
        StatusCode = statusCode;
    }

    public override T Value { get; }

    public override HttpStatusCode StatusCode { get; }
}
