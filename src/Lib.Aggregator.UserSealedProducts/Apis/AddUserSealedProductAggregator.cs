using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSealedProducts.Apis;

public sealed class AddUserSealedProductAggregator : IAddUserSealedProductAggregator
{
    private readonly Commands.AddUserSealedProductAggregator _aggregator;

    public AddUserSealedProductAggregator(ILogger logger) : this(
        new Commands.AddUserSealedProductAggregator(logger))
    { }

    private AddUserSealedProductAggregator(Commands.AddUserSealedProductAggregator aggregator) =>
        _aggregator = aggregator;

    public Task<IOperationResponse<IUserSealedProductOufEntity>> Execute(IAddUserSealedProductItrEntity input) =>
        _aggregator.Execute(input);
}
