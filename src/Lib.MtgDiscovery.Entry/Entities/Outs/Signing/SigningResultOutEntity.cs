using System.Collections.Generic;

namespace Lib.MtgDiscovery.Entry.Entities.Outs.Signing;

public sealed class SigningResultOutEntity
{
    public ICollection<SigningSetGroupOutEntity> Sets { get; init; }
}
