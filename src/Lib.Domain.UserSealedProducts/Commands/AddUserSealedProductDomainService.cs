using System.Threading.Tasks;
using Lib.Aggregator.UserSealedProducts.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserSealedProducts.Commands;

/// <summary>
/// Single-method service for adding a user sealed product to the collection.
/// Delegates to aggregator layer for data persistence.
/// Future: Could add business rules such as maximum quantity limits or product validation.
/// </summary>
internal sealed class AddUserSealedProductDomainService : IAddUserSealedProductDomainService
{
    private readonly IAddUserSealedProductAggregator _aggregator;

    public AddUserSealedProductDomainService(ILogger logger) : this(new AddUserSealedProductAggregator(logger))
    { }

    private AddUserSealedProductDomainService(IAddUserSealedProductAggregator aggregator) => _aggregator = aggregator;

    public async Task<IOperationResponse<IUserSealedProductOufEntity>> Execute(IAddUserSealedProductItrEntity input) => await _aggregator.Execute(input).ConfigureAwait(false);
}
