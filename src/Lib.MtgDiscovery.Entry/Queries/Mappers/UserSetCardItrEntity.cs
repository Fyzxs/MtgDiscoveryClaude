using Lib.Aggregator.UserSetCards.Entities;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class UserSetCardItrEntity : IUserSetCardItrEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
}