using System.Net;

namespace Lib.Shared.Invocation.Commands;

public interface ICommandOperationStatusContent
{
    string Message();
    HttpStatusCode Status();
}
