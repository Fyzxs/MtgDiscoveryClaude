using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Adapter.Collections.Exceptions;

#pragma warning disable CA1032 // Implement standard exception constructors
public sealed class CollectionAdapterException : OperationException
#pragma warning restore CA1032
{
    public CollectionAdapterException(string message) : base(HttpStatusCode.InternalServerError, message) { }
    public CollectionAdapterException(string message, Exception innerException) : base(HttpStatusCode.InternalServerError, message, innerException) { }
}
