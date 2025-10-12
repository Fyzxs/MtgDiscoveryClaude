using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Adapter.UserSetCards.Commands.Entities;

internal sealed class AddSetGroupToUserSetCardXfrEntity : IAddSetGroupToUserSetCardXfrEntity
{
    public string UserId { get; init; } = string.Empty;
    public string SetId { get; init; } = string.Empty;
    public string SetGroupId { get; init; } = string.Empty;
    public bool Collecting { get; init; }
    public int Count { get; init; }
}
