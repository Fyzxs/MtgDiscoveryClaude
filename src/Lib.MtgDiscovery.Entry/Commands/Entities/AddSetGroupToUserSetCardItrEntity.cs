using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Commands.Entities;

internal sealed class AddSetGroupToUserSetCardItrEntity : IAddSetGroupToUserSetCardItrEntity
{
    public required string UserId { get; init; }
    public required string SetId { get; init; }
    public required string SetGroupId { get; init; }
    public required bool Collecting { get; init; }
    public required int Count { get; init; }
}
