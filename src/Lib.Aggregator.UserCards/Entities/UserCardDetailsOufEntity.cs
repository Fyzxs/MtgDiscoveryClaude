using Lib.Shared.DataModels.Entities.Oufs.UserCards;

namespace Lib.Aggregator.UserCards.Entities;

internal sealed class UserCardDetailsOufEntity : IUserCardDetailsOufEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}
