using Lib.Shared.Invocation.Commands;
using Lib.Shared.Invocation.Response.Models;

namespace Lib.Shared.Invocation.Response;

public interface ICommandResponseModelFactory
{
    public ResponseModel Success(CommandOperationStatus commandOperationStatus);
    public ResponseModel Failure(CommandOperationStatus commandOperationStatus);
}
