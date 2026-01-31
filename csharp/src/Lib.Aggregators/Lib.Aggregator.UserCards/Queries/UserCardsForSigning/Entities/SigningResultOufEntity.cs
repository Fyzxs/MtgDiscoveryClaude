using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;

namespace Lib.Aggregator.UserCards.Queries.UserCardsForSigning.Entities;

internal sealed class SigningResultOufEntity : ISigningResultOufEntity
{
    public IEnumerable<ISigningSetGroupOufEntity> Sets { get; init; }
}
