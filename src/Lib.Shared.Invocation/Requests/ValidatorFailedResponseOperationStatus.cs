using System.Net;
using Lib.Shared.Invocation.Queries;

namespace Lib.Shared.Invocation.Requests;

internal sealed class ValidatorFailedResponseOperationStatus<TResponseType> : RequestOperationStatus<TResponseType>
{
    public ValidatorFailedResponseOperationStatus(string message) : base(new ValidatorRequestException(message))
    { }

    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
