using Lib.Shared.Invocation.Primitives;

namespace Lib.Shared.Invocation;

public interface ICaller
{
    bool IsAuthenticated();
    bool IsNotAuthenticated();
    PrincipalId Id();
}