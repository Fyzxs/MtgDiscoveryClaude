using Lib.Shared.DataModels.Entities.Args;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserCards;

public sealed class UserCardDetailsArgEntity : IUserCardDetailsArgEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
    public string SetGroupId { get; init; }
}
