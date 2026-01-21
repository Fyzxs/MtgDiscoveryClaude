using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Adapter.SealedProducts.Exceptions;

#pragma warning disable CA1032 // Implement standard exception constructors
public sealed class SealedProductsAdapterException : OperationException
#pragma warning restore CA1032
{
    public SealedProductsAdapterException(string message) : base(HttpStatusCode.InternalServerError, message) { }
    public SealedProductsAdapterException(string message, Exception innerException) : base(HttpStatusCode.InternalServerError, message, innerException) { }
}
