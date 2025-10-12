using Lib.Shared.DataModels.Entities.Args;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserCards;

public sealed class UserCardsBySetArgEntity : IUserCardsBySetArgEntity
{
    public string SetId { get; init; }
    public string UserId { get; init; }
}
