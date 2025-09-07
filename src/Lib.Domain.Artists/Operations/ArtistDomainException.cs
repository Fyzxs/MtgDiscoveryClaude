using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Domain.Artists.Operations;

#pragma warning disable CA1032
internal sealed class ArtistDomainException : OperationException
#pragma warning restore CA1032
{
    public ArtistDomainException(string message, Exception innerException = null)
        : base(HttpStatusCode.InternalServerError, message, innerException)
    { }
}
