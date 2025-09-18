using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Args;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserCards;

public sealed class UserCardArgEntity : IUserCardArgEntity
{
    public string CardId { get; init; }
    public string SetId { get; init; }

    // GraphQL needs concrete type for input, not interface
    public UserCardDetailsArgEntity UserCardDetails { get; init; }

    // Implement interface property
    IUserCardDetailsArgEntity IUserCardArgEntity.UserCardDetails => UserCardDetails;
}
