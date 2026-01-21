using Lib.Shared.DataModels.Entities.Args;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserCards;

public sealed class UserCardArgEntity : IUserCardArgEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
}
