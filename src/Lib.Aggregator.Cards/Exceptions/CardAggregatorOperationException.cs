using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Aggregator.Cards.Exceptions;

#pragma warning disable CA1032
internal sealed class CardAggregatorOperationException : OperationException
#pragma warning restore CA1032
{
    public CardAggregatorOperationException(string message, Exception innerException = null)
        : base(HttpStatusCode.InternalServerError, message, innerException)
    {
    }
}