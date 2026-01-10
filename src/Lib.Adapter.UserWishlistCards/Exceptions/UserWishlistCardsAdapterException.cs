using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Adapter.UserWishlistCards.Exceptions;

public sealed class UserWishlistCardsAdapterException : OperationException
{
    public UserWishlistCardsAdapterException() : base(HttpStatusCode.InternalServerError, "UserWishlistCards adapter operation failed")
    {
    }

    public UserWishlistCardsAdapterException(string message) : base(HttpStatusCode.InternalServerError, message)
    {
    }

    public UserWishlistCardsAdapterException(string message, Exception innerException) : base(HttpStatusCode.InternalServerError, message, innerException)
    {
    }
}
