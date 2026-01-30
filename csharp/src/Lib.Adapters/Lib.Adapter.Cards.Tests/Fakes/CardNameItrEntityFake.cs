using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.Cards.Tests.Fakes;

internal sealed class CardNameItrEntityFake : ICardNameItrEntity
{
    public string CardName { get; init; } = string.Empty;
}
