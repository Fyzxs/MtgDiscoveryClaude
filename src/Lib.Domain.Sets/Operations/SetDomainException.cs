using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Domain.Sets.Operations;

internal sealed class SetDomainException : OperationException
{
    public SetDomainException()
        : base(HttpStatusCode.InternalServerError, "Set domain operation failed")
    { }

    public SetDomainException(string message)
        : base(HttpStatusCode.InternalServerError, message)
    { }

    public SetDomainException(string message, Exception innerException)
        : base(HttpStatusCode.InternalServerError, message, innerException)
    { }
}
