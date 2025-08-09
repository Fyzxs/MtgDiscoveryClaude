using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace Lib.Universal.Extensions;

public static class ExceptionExtensions
{
    public static Exception ThrowMe(this Exception ex)
    {
        ExceptionDispatchInfo.Capture(ex).Throw();
        return new UnreachableException("The compiler doesn't understand the semantics of dispatcher.");
    }
}
