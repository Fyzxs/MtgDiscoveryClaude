using System.Collections.Generic;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class FakeCardItemCollectionItrEntity : ICardItemCollectionItrEntity
{
    public ICollection<ICardItemItrEntity> Data { get; init; } = [];
}