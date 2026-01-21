using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.UserCards.Entities;

internal sealed class UserCardDetailsItrEntity : IUserCardDetailsItrEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}