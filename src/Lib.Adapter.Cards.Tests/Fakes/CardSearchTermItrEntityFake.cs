using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.Cards.Tests.Fakes;

internal sealed class CardSearchTermItrEntityFake : ICardSearchTermItrEntity
{
    public string SearchTerm { get; init; } = string.Empty;
}
