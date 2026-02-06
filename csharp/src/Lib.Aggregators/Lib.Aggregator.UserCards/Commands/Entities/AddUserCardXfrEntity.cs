using System.Collections.Generic;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Aggregator.UserCards.Commands.Entities;

internal sealed class AddUserCardXfrEntity : IAddUserCardXfrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public string CardName { get; init; }
    public string SetName { get; init; }
    public string SetCode { get; init; }
    public string ReleasedAt { get; init; }
    public string Artist { get; init; }
    public IEnumerable<string> ArtistIds { get; init; }
    public string CardNameGuid { get; init; }
    public IUserCardDetailsXfrEntity Details { get; init; }
    public bool ReplaceMode { get; init; }
    public string CacheKey => $"add_user_card:{UserId}:{CardId}";
}
