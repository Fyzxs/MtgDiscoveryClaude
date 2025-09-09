using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.Cards.Internals;

internal sealed class CardNameSearchResultItrEntity : ICardNameSearchResultItrEntity
{
    public string Name { get; init; } = string.Empty;
}