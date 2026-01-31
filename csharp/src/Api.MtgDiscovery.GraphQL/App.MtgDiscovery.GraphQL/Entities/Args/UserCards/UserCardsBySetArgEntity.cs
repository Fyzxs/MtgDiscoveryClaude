using Lib.Shared.DataModels.Entities.Args.UserCards;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserCards;

internal sealed class UserCardsBySetArgEntity : IUserCardsBySetArgEntity
{
    public string SetId { get; init; }
    public string UserId { get; init; }
}
