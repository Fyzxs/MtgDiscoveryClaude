using Lib.Shared.DataModels.Entities.Args.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserSetCards;

internal sealed class AllUserSetCardsArgEntity : IAllUserSetCardsArgEntity
{
    public string UserId { get; init; }
}
