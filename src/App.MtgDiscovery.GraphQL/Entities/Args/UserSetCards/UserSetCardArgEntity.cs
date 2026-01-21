using Lib.Shared.DataModels.Entities.Args;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserSetCards;

public sealed class UserSetCardArgEntity : IUserSetCardArgEntity
{
    public string UserId { get; init; }
    public string SetId { get; init; }
}