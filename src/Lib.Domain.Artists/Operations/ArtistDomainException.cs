using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Domain.Artists.Operations;

internal sealed class ArtistDomainException : OperationException
{
    public ArtistDomainException()
        : base(HttpStatusCode.InternalServerError, "Artist domain operation failed")
    { }

    public ArtistDomainException(string message)
        : base(HttpStatusCode.InternalServerError, message)
    { }

    public ArtistDomainException(string message, Exception innerException)
        : base(HttpStatusCode.InternalServerError, message, innerException)
    { }
}
