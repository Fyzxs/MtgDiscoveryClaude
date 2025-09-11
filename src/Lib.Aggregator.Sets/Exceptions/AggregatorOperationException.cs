using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Aggregator.Sets.Exceptions;

#pragma warning disable CA1032
internal sealed class AggregatorOperationException : OperationException
#pragma warning restore CA1032
{
    public AggregatorOperationException(HttpStatusCode statusCode, string message, Exception innerException = null)
        : base(statusCode, message, innerException)
    { }
}
