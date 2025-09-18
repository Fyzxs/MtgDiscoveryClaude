using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Aggregator.UserCards.Commands.Entities;

internal sealed class UserCardDetailsXfrEntity : IUserCardDetailsXfrEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}
