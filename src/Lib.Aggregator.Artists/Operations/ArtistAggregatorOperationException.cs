using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Aggregator.Artists.Operations;

#pragma warning disable CA1032
internal sealed class ArtistAggregatorOperationException : OperationException
#pragma warning restore CA1032
{
    public ArtistAggregatorOperationException(string message, Exception innerException = null)
        : base(HttpStatusCode.InternalServerError, message, innerException)
    { }
}
