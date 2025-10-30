using Lib.Shared.DataModels.Entities.Args;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserSetCards;

public sealed class AllUserSetCardsArgEntity : IAllUserSetCardsArgEntity
{
    public string UserId { get; init; }
}
