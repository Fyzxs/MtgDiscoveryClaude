using Lib.Shared.DataModels.Entities.Args.UserCards;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserCards;

internal sealed class UserCardArgEntity : IUserCardArgEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
}
