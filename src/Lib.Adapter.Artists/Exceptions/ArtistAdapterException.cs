using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Adapter.Artists.Exceptions;

#pragma warning disable CA1032 // Implement standard exception constructors
public sealed class ArtistAdapterException : OperationException
#pragma warning restore CA1032
{
    public ArtistAdapterException(string message) : base(HttpStatusCode.InternalServerError, message) { }
    public ArtistAdapterException(string message, Exception innerException) : base(HttpStatusCode.InternalServerError, message, innerException) { }
}
