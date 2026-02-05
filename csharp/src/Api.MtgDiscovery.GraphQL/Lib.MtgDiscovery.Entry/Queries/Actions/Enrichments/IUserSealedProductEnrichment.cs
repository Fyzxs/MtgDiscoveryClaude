using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Enrichments;

internal interface IUserSealedProductEnrichment
{
    Task Enrich(List<SealedProductOutEntity> outEntities, ISealedProductsBySetCodeArgEntity args, CancellationToken cancellationToken);
}
