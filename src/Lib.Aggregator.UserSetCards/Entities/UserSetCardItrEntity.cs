namespace Lib.Aggregator.UserSetCards.Entities;

internal sealed class UserSetCardItrEntity : IUserSetCardItrEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
}
