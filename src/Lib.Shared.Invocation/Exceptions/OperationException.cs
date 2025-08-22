using System;
using System.Net;

namespace Lib.Shared.Invocation.Exceptions;

#pragma warning disable CA1032 // Implement standard exception constructors
public abstract class OperationException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
{
    protected OperationException(HttpStatusCode statusCode, string statusMessage, Exception innerException = null) : base($"[status={statusCode}] [message={statusMessage}]", innerException)
    {
        StatusCode = statusCode;
        StatusMessage = statusMessage;
    }

    public HttpStatusCode StatusCode { get; }

    public string StatusMessage { get; }
}
