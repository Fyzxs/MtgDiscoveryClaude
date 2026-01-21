using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Aggregator.Sets.Exceptions;

#pragma warning disable CA1032
internal sealed class SetsAggregatorOperationException : OperationException
#pragma warning restore CA1032
{
    public SetsAggregatorOperationException(string message, Exception innerException = null)
        : base(HttpStatusCode.InternalServerError, message, innerException)
    { }
}
