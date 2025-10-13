using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Domain.UserCards.Operations;

internal sealed class UserCardsDomainException : OperationException
{
    public UserCardsDomainException()
        : base(HttpStatusCode.InternalServerError, "User cards domain operation failed")
    { }

    public UserCardsDomainException(string message)
        : base(HttpStatusCode.InternalServerError, message)
    { }

    public UserCardsDomainException(string message, Exception innerException)
        : base(HttpStatusCode.InternalServerError, message, innerException)
    { }
}
