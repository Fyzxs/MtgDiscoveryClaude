using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Entities;

internal sealed class CollectedItemItrEntity : ICollectedItemItrEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}
