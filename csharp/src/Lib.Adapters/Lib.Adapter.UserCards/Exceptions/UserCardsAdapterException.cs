using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Adapter.UserCards.Exceptions;

#pragma warning disable CA1032 // Implement standard exception constructors
public sealed class UserCardsAdapterException : OperationException
#pragma warning restore CA1032
{
    public UserCardsAdapterException(string message) : base(HttpStatusCode.InternalServerError, message) { }
    public UserCardsAdapterException(string message, Exception innerException) : base(HttpStatusCode.InternalServerError, message, innerException) { }
}
