using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Shared.Invocation.Requests;

#pragma warning disable CA1032 // Add all constructors
internal sealed class ValidatorRequestException : RequestException
#pragma warning restore CA1032  
{
    public ValidatorRequestException(string message) : base(HttpStatusCode.BadRequest, message)
    { }
}
