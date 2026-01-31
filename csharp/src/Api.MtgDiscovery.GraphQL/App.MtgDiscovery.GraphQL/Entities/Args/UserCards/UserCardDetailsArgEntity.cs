using Lib.Shared.DataModels.Entities.Args.UserCards;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserCards;

internal sealed class UserCardDetailsArgEntity : IUserCardDetailsArgEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
    public string SetGroupId { get; init; }
}
