using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Adapter.UserWishlistCards.Exceptions;

#pragma warning disable CA1032 // Implement standard exception constructors
public sealed class UserWishlistCardsAdapterException : OperationException
#pragma warning restore CA1032
{
    public UserWishlistCardsAdapterException(string message) : base(HttpStatusCode.InternalServerError, message) { }
    public UserWishlistCardsAdapterException(string message, Exception innerException) : base(HttpStatusCode.InternalServerError, message, innerException) { }
}
