using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Domain.User.Operations;

internal sealed class UserDomainException : OperationException
{
    public UserDomainException()
        : base(HttpStatusCode.InternalServerError, "User domain operation failed")
    { }

    public UserDomainException(string message)
        : base(HttpStatusCode.InternalServerError, message)
    { }

    public UserDomainException(string message, Exception innerException)
        : base(HttpStatusCode.InternalServerError, message, innerException)
    { }
}
