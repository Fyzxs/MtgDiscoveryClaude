using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Abstractions.Actions.Integrators;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Integrators;

internal interface IUserCardCollectionIntegrator : IIntegrator<List<CardItemOutEntity>, IEnumerable<IUserCardOufEntity>>
{
}
