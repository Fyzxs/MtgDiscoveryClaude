using Lib.Adapter.Cards.Apis.Entities;

namespace Lib.Aggregator.Cards.Queries.Entities;

internal sealed class SetCodeXfrEntity : ISetCodeXfrEntity
{
    public string SetCode { get; init; }
}