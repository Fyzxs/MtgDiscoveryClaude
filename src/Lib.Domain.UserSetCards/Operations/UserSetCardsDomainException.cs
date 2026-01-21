using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Domain.UserSetCards.Operations;

internal sealed class UserSetCardsDomainException : OperationException
{
    public UserSetCardsDomainException()
        : base(HttpStatusCode.InternalServerError, "User set cards domain operation failed")
    { }

    public UserSetCardsDomainException(string message)
        : base(HttpStatusCode.InternalServerError, message)
    { }

    public UserSetCardsDomainException(string message, Exception innerException)
        : base(HttpStatusCode.InternalServerError, message, innerException)
    { }
}
