using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Adapter.Cards.Exceptions;

#pragma warning disable CA1032 // Implement standard exception constructors
public sealed class CardAdapterException : OperationException
#pragma warning restore CA1032
{
    public CardAdapterException(string message) : base(HttpStatusCode.InternalServerError, message) { }
    public CardAdapterException(string message, Exception innerException) : base(HttpStatusCode.InternalServerError, message, innerException) { }
}
