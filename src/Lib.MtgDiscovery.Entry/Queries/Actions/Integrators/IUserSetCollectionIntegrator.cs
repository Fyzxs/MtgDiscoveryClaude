using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.Abstractions.Actions.Integrators;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Integrators;

internal interface IUserSetCollectionIntegrator : IIntegrator<List<SetItemOutEntity>, IEnumerable<IUserSetCardOufEntity>>
{
}
