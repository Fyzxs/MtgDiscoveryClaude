using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Adapter.User.Exceptions;

#pragma warning disable CA1032 // Implement standard exception constructors
public sealed class UserAdapterException : OperationException
#pragma warning restore CA1032
{
    public UserAdapterException(string message) : base(HttpStatusCode.InternalServerError, message) { }
    public UserAdapterException(string message, Exception innerException) : base(HttpStatusCode.InternalServerError, message, innerException) { }
}
