using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;

namespace Lib.MtgDiscovery.Entry.Entities.Outs.Signing;

public sealed class SigningCardOutEntity
{
    public string CardId { get; init; }
    public string CardName { get; init; }
    public string ImageUri { get; init; }
    public int UnsignedCopies { get; init; }
    public bool IsPartiallySigned { get; init; }
    public bool IsFullySigned { get; init; }
    public ICollection<CollectedItemOutEntity> CollectedDetails { get; init; }
}
