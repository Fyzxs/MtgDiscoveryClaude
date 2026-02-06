using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Janitors;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Adapter.UserWishlistCards.Commands.Integrators;
using Lib.Adapter.UserWishlistCards.Commands.Mappers;
using Lib.Adapter.UserWishlistCards.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserWishlistCards.Commands;

internal sealed class RemoveUserWishlistCardAdapter : IRemoveUserWishlistCardAdapter
{
    private readonly ICosmosGopher _userWishlistCardsGopher;
    private readonly ICosmosScribe _userWishlistCardsScribe;
    private readonly ICosmosJanitor _userWishlistCardsJanitor;
    private readonly IRemoveUserWishlistCardXfrToReadPointMapper _readPointMapper;
    private readonly IRemoveUserWishlistCardXfrToDeletePointMapper _deletePointMapper;
    private readonly IRemoveUserWishlistCardIntegrator _integrator;

    public RemoveUserWishlistCardAdapter(ILogger logger) : this(
        new UserWishlistCardsGopher(logger),
        new UserWishlistCardsScribe(logger),
        new UserWishlistCardsJanitor(logger),
        new RemoveUserWishlistCardXfrToReadPointMapper(),
        new RemoveUserWishlistCardXfrToDeletePointMapper(),
        new RemoveUserWishlistCardIntegrator())
    { }

    private RemoveUserWishlistCardAdapter(
        ICosmosGopher userWishlistCardsGopher,
        ICosmosScribe userWishlistCardsScribe,
        ICosmosJanitor userWishlistCardsJanitor,
        IRemoveUserWishlistCardXfrToReadPointMapper readPointMapper,
        IRemoveUserWishlistCardXfrToDeletePointMapper deletePointMapper,
        IRemoveUserWishlistCardIntegrator integrator)
    {
        _userWishlistCardsGopher = userWishlistCardsGopher;
        _userWishlistCardsScribe = userWishlistCardsScribe;
        _userWishlistCardsJanitor = userWishlistCardsJanitor;
        _readPointMapper = readPointMapper;
        _deletePointMapper = deletePointMapper;
        _integrator = integrator;
    }

    public async Task<IOperationResponse<UserWishlistCardExtEntity>> Execute(
        [NotNull] IRemoveUserWishlistCardXfrEntity input,
        CancellationToken cancellationToken)
    {
        ReadPointItem readPoint = await _readPointMapper.Map(input).ConfigureAwait(false);

        OpResponse<UserWishlistCardExtEntity> existingResponse = await _userWishlistCardsGopher
            .ReadAsync<UserWishlistCardExtEntity>(readPoint, cancellationToken).ConfigureAwait(false);

        if (existingResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<UserWishlistCardExtEntity>(
                new UserWishlistCardsAdapterException($"Wishlist card not found: {input.CardId}"));
        }

        UserWishlistCardExtEntity existingItem = existingResponse.Value;
        UserWishlistCardExtEntity updatedItem = await _integrator.Integrate(existingItem, input).ConfigureAwait(false);

        if (updatedItem.WishlistItems.Any() is false)
        {
            DeletePointItem deletePoint = await _deletePointMapper.Map(input).ConfigureAwait(false);

            OpResponse<UserWishlistCardExtEntity> deleteResponse = await _userWishlistCardsJanitor
                .DeleteAsync<UserWishlistCardExtEntity>(deletePoint, cancellationToken).ConfigureAwait(false);

            if (deleteResponse.IsNotSuccessful())
            {
                return new FailureOperationResponse<UserWishlistCardExtEntity>(
                    new UserWishlistCardsAdapterException($"Failed to delete wishlist card: {deleteResponse.StatusCode}"));
            }

            return new SuccessOperationResponse<UserWishlistCardExtEntity>(existingItem);
        }

        OpResponse<UserWishlistCardExtEntity> upsertResponse = await _userWishlistCardsScribe
            .UpsertAsync(updatedItem, cancellationToken).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<UserWishlistCardExtEntity>(
                new UserWishlistCardsAdapterException($"Failed to remove user wishlist card: {upsertResponse.StatusCode}"));
        }

        return new SuccessOperationResponse<UserWishlistCardExtEntity>(upsertResponse.Value);
    }
}
