using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.MtgDiscovery.Entry.Queries.SealedProducts;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Apis;

internal sealed class SealedProductsEntryService : ISealedProductsEntryService
{
    private readonly ISealedProductsBySetCodeEntryService _sealedProductsBySetCodeEntryService;

    public SealedProductsEntryService(ILogger logger) : this(new SealedProductsBySetCodeEntryService(logger))
    { }

    private SealedProductsEntryService(ISealedProductsBySetCodeEntryService sealedProductsBySetCodeEntryService) => _sealedProductsBySetCodeEntryService = sealedProductsBySetCodeEntryService;

    public Task<IOperationResponse<List<SealedProductOutEntity>>> SealedProductsBySetCodeAsync(ISealedProductsBySetCodeArgEntity args) => _sealedProductsBySetCodeEntryService.Execute(args);
}
