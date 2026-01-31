using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;
using Lib.MtgDiscovery.Entry.Queries.Actions.Integrators;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Enrichments;

internal sealed class UserSealedProductEnrichment : IUserSealedProductEnrichment
{
    private readonly IUserSealedProductsEntryService _userSealedProductsEntryService;
    private readonly IUserSealedProductIntegrator _integrator;

    public UserSealedProductEnrichment(ILogger logger) : this(
        new UserSealedProductsEntryService(logger),
        new UserSealedProductIntegrator())
    {
    }

    private UserSealedProductEnrichment(
        IUserSealedProductsEntryService userSealedProductsEntryService,
        IUserSealedProductIntegrator integrator)
    {
        _userSealedProductsEntryService = userSealedProductsEntryService;
        _integrator = integrator;
    }

    public async Task Enrich(List<SealedProductOutEntity> target, ISealedProductsBySetCodeArgEntity args)
    {
        if (args.HasCollectionId is false)
            return;

        IOperationResponse<List<UserSealedProductOutEntity>> userProductsResponse =
            await _userSealedProductsEntryService.GetUserSealedProductsByUserIdAsync(args.CollectionId).ConfigureAwait(false);

        if (userProductsResponse.IsFailure)
            return;

        _ = await _integrator.Integrate(target, userProductsResponse.ResponseData).ConfigureAwait(false);
    }
}
