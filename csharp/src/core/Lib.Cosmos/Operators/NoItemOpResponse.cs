using System;
using System.Net;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Operators;

internal sealed class CosmosExceptionOpResponse<T> : OpResponse<T>
{
    private readonly CosmosException _exception;
    public CosmosExceptionOpResponse(CosmosException cosmosException) => _exception = cosmosException;

    public override T Value => throw new InvalidOperationException("No value available when unsuccessful", _exception);

    public override HttpStatusCode StatusCode => _exception.StatusCode;

    public override CosmosException Exception() => _exception;
}
