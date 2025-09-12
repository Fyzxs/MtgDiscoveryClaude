using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Adapter.Sets.Exceptions;

#pragma warning disable CA1032 // Implement standard exception constructors
public sealed class SetAdapterException : OperationException
#pragma warning restore CA1032
{
    public SetAdapterException(string message) : base(HttpStatusCode.InternalServerError, message) { }
    public SetAdapterException(string message, Exception innerException) : base(HttpStatusCode.InternalServerError, message, innerException) { }
}
