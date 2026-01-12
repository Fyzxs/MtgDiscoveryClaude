using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Adapter.UserSealedProducts.Exceptions;

/// <summary>
/// Exception thrown when user sealed products adapter operations fail.
/// </summary>
internal sealed class UserSealedProductsAdapterException : OperationException
{
    public UserSealedProductsAdapterException()
        : base(HttpStatusCode.InternalServerError, "UserSealedProducts adapter operation failed")
    { }

    public UserSealedProductsAdapterException(string message)
        : base(HttpStatusCode.InternalServerError, message)
    { }

    public UserSealedProductsAdapterException(string message, Exception innerException)
        : base(HttpStatusCode.InternalServerError, message, innerException)
    { }
}
