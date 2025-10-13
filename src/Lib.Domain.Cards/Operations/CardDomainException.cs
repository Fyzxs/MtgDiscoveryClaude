using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Domain.Cards.Operations;

internal sealed class CardDomainException : OperationException
{
    public CardDomainException()
        : base(HttpStatusCode.InternalServerError, "Card domain operation failed")
    { }

    public CardDomainException(string message)
        : base(HttpStatusCode.InternalServerError, message)
    { }

    public CardDomainException(string message, Exception innerException)
        : base(HttpStatusCode.InternalServerError, message, innerException)
    { }
}
