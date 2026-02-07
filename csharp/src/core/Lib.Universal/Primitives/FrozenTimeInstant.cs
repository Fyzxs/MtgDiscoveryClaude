using System;

namespace Lib.Universal.Primitives;

public sealed class FrozenTimeInstant : TimeInstant
{
    private readonly DateTime _frozen;

    public FrozenTimeInstant(DateTime frozen) => _frozen = frozen;

    public override DateTime AsSystemType() => _frozen;
}
