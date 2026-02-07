using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Adapter.UserSealedProducts.Exceptions;

#pragma warning disable CA1032 // Implement standard exception constructors
public sealed class UserSealedProductsAdapterException : OperationException
#pragma warning restore CA1032
{
    public UserSealedProductsAdapterException(string message) : base(HttpStatusCode.InternalServerError, message) { }
    public UserSealedProductsAdapterException(string message, Exception innerException) : base(HttpStatusCode.InternalServerError, message, innerException) { }
}
