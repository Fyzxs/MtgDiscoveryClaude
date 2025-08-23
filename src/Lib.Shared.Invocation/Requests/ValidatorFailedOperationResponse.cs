using System.Net;
using Lib.Shared.Invocation.Operations;

namespace Lib.Shared.Invocation.Requests;

internal sealed class ValidatorFailedOperationResponse<TResponseType> : OperationResponse<TResponseType>
{
    public ValidatorFailedOperationResponse(string message) : base(new ValidatorOperationException(message))
    {
        Status = HttpStatusCode.BadRequest;
    }
}
