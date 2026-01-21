using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserCards;

public sealed class UserCardsSetArgEntity : IUserCardsSetArgEntity
{
    public string SetId { get; init; }
    public string UserId { get; init; }
}
