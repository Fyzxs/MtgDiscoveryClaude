using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;

namespace Lib.Aggregator.UserCards.Queries.UserCardsForSigning.Entities;

internal sealed class SigningCardOufEntity : ISigningCardOufEntity
{
    public string CardId { get; init; }
    public string CardName { get; init; }
    public string ImageUri { get; init; }
    public int UnsignedCopies { get; init; }
    public bool IsPartiallySigned { get; init; }
    public bool IsFullySigned { get; init; }
    public IEnumerable<IUserCardDetailsOufEntity> CollectedDetails { get; init; }
}
