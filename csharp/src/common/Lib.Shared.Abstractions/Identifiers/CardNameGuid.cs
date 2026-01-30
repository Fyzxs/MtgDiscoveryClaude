using System;
using Lib.Universal.Primitives;

namespace Lib.Shared.Abstractions.Identifiers;

public sealed class CardNameGuid : ToSystemType<Guid>
{
    private readonly Guid _origin;

    public CardNameGuid(Guid origin) => _origin = origin;

    public override Guid AsSystemType() => _origin;
}
