using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Adapter.UserSetCards.Exceptions;

#pragma warning disable CA1032 // Implement standard exception constructors
public sealed class UserSetCardsAdapterException : OperationException
#pragma warning restore CA1032
{
    public UserSetCardsAdapterException(string message) : base(HttpStatusCode.InternalServerError, message) { }
    public UserSetCardsAdapterException(string message, Exception innerException) : base(HttpStatusCode.InternalServerError, message, innerException) { }
}
