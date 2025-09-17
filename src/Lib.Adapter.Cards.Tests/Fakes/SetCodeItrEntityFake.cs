using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.Cards.Tests.Fakes;

internal sealed class SetCodeItrEntityFake : ISetCodeItrEntity
{
    public string SetCode { get; init; } = string.Empty;
}
