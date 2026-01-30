using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Shared.Invocation.Requests;

#pragma warning disable CA1032 // Add all constructors
internal sealed class ValidatorOperationException : OperationException
#pragma warning restore CA1032  
{
    public ValidatorOperationException(string message) : base(HttpStatusCode.BadRequest, message)
    { }
}
