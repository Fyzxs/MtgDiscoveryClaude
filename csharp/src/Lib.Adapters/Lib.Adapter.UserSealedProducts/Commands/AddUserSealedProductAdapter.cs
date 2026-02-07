using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SealedProducts;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Janitors;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Adapter.UserSealedProducts.Commands.Integrators;
using Lib.Adapter.UserSealedProducts.Commands.Mappers;
using Lib.Adapter.UserSealedProducts.Commands.Resolvers;
using Lib.Adapter.UserSealedProducts.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSealedProducts.Commands;

internal sealed class AddUserSealedProductAdapter : IAddUserSealedProductAdapter
{
    private readonly ICosmosGopher _sealedProductsGopher;
    private readonly ICosmosGopher _userSealedProductsGopher;
    private readonly ICosmosScribe _userSealedProductsScribe;
    private readonly ICosmosJanitor _userSealedProductsJanitor;
    private readonly IUserSealedProductXfrToProductReadPointMapper _productReadPointMapper;
    private readonly IUserSealedProductXfrToReadPointMapper _readPointMapper;
    private readonly IUserSealedProductXfrToDeletePointMapper _deletePointMapper;
    private readonly IUserSealedProductResolver _resolver;
    private readonly IUserSealedProductIntegrator _integrator;

    public AddUserSealedProductAdapter(ILogger logger)
        : this(
            new SealedProductsGopher(logger),
            new UserSealedProductsGopher(logger),
            new UserSealedProductsScribe(logger),
            new UserSealedProductsJanitor(logger),
            new UserSealedProductXfrToProductReadPointMapper(),
            new UserSealedProductXfrToReadPointMapper(),
            new UserSealedProductXfrToDeletePointMapper(),
            new UserSealedProductResolver(),
            new UserSealedProductIntegrator())
    { }

    private AddUserSealedProductAdapter(
        ICosmosGopher sealedProductsGopher,
        ICosmosGopher userSealedProductsGopher,
        ICosmosScribe userSealedProductsScribe,
        ICosmosJanitor userSealedProductsJanitor,
        IUserSealedProductXfrToProductReadPointMapper productReadPointMapper,
        IUserSealedProductXfrToReadPointMapper readPointMapper,
        IUserSealedProductXfrToDeletePointMapper deletePointMapper,
        IUserSealedProductResolver resolver,
        IUserSealedProductIntegrator integrator)
    {
        _sealedProductsGopher = sealedProductsGopher;
        _userSealedProductsGopher = userSealedProductsGopher;
        _userSealedProductsScribe = userSealedProductsScribe;
        _userSealedProductsJanitor = userSealedProductsJanitor;
        _productReadPointMapper = productReadPointMapper;
        _readPointMapper = readPointMapper;
        _deletePointMapper = deletePointMapper;
        _resolver = resolver;
        _integrator = integrator;
    }

    public async Task<IOperationResponse<UserSealedProductExtEntity>> Execute(
        [NotNull] IUserSealedProductXfrEntity input, CancellationToken cancellationToken)
    {
        ReadPointItem productReadPoint = await _productReadPointMapper.Map(input).ConfigureAwait(false);

        OpResponse<SealedProductExtEntity> productResponse =
            await _sealedProductsGopher.ReadAsync<SealedProductExtEntity>(productReadPoint, cancellationToken).ConfigureAwait(false);

        if (productResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<UserSealedProductExtEntity>(
                new UserSealedProductsAdapterException(
                    $"Failed to fetch sealed product details: {productResponse.StatusCode}"));
        }

        SealedProductExtEntity product = productResponse.Value;

        ReadPointItem userProductReadPoint = await _readPointMapper.Map(input).ConfigureAwait(false);

        OpResponse<UserSealedProductExtEntity> existingResponse =
            await _userSealedProductsGopher.ReadAsync<UserSealedProductExtEntity>(userProductReadPoint, cancellationToken).ConfigureAwait(false);

        UserSealedProductExtEntity current = _resolver.Resolve(existingResponse, input);

        UserSealedProductExtEntity updated = _integrator.Integrate(current, input, product);

        if (updated.Count < 1)
        {
            if (existingResponse.IsSuccessful())
            {
                DeletePointItem deletePoint = await _deletePointMapper.Map(input).ConfigureAwait(false);

                OpResponse<UserSealedProductExtEntity> deleteResponse =
                    await _userSealedProductsJanitor.DeleteAsync<UserSealedProductExtEntity>(deletePoint, cancellationToken).ConfigureAwait(false);

                if (deleteResponse.IsNotSuccessful())
                {
                    return new FailureOperationResponse<UserSealedProductExtEntity>(
                        new UserSealedProductsAdapterException(
                            $"Failed to delete user sealed product: {deleteResponse.StatusCode}"));
                }
            }

            return new SuccessOperationResponse<UserSealedProductExtEntity>(updated);
        }

        OpResponse<UserSealedProductExtEntity> upsertResponse =
            await _userSealedProductsScribe.UpsertAsync(updated, cancellationToken).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<UserSealedProductExtEntity>(
                new UserSealedProductsAdapterException(
                    $"Failed to add user sealed product: {upsertResponse.StatusCode}"));
        }

        return new SuccessOperationResponse<UserSealedProductExtEntity>(upsertResponse.Value);
    }
}
