using System;

namespace Lib.Universal.Primitives;

public sealed class UtcNowTimeInstant : TimeInstant
{
    public override DateTime AsSystemType() => DateTime.UtcNow;
}
