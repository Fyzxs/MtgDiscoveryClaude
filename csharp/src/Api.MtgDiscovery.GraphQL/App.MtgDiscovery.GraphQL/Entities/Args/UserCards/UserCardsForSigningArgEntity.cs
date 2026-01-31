using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Args.UserCards;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserCards;

internal sealed class UserCardsForSigningArgEntity : IUserCardsForSigningArgEntity
{
    public string UserId { get; init; }
    public ICollection<string> ArtistIds { get; init; }
}
