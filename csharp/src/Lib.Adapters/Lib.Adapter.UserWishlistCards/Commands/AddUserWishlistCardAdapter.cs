using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Adapter.UserWishlistCards.Commands.Integrators;
using Lib.Adapter.UserWishlistCards.Commands.Mappers;
using Lib.Adapter.UserWishlistCards.Commands.Resolvers;
using Lib.Adapter.UserWishlistCards.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserWishlistCards.Commands;

internal sealed class AddUserWishlistCardAdapter : IAddUserWishlistCardAdapter
{
    private readonly ICosmosGopher _userWishlistCardsGopher;
    private readonly ICosmosScribe _userWishlistCardsScribe;
    private readonly IAddUserWishlistCardXfrToReadPointMapper _readPointMapper;
    private readonly IUserWishlistCardResolver _resolver;
    private readonly IUserWishlistCardIntegrator _integrator;

    public AddUserWishlistCardAdapter(ILogger logger) : this(
        new UserWishlistCardsGopher(logger),
        new UserWishlistCardsScribe(logger),
        new AddUserWishlistCardXfrToReadPointMapper(),
        new UserWishlistCardResolver(),
        new UserWishlistCardIntegrator())
    { }

    private AddUserWishlistCardAdapter(
        ICosmosGopher userWishlistCardsGopher,
        ICosmosScribe userWishlistCardsScribe,
        IAddUserWishlistCardXfrToReadPointMapper readPointMapper,
        IUserWishlistCardResolver resolver,
        IUserWishlistCardIntegrator integrator)
    {
        _userWishlistCardsGopher = userWishlistCardsGopher;
        _userWishlistCardsScribe = userWishlistCardsScribe;
        _readPointMapper = readPointMapper;
        _resolver = resolver;
        _integrator = integrator;
    }

    public async Task<IOperationResponse<UserWishlistCardExtEntity>> Execute(
        [NotNull] IAddUserWishlistCardXfrEntity input,
        CancellationToken cancellationToken)
    {
        ReadPointItem readPoint = await _readPointMapper.Map(input).ConfigureAwait(false);

        OpResponse<UserWishlistCardExtEntity> readResponse = await _userWishlistCardsGopher
            .ReadAsync<UserWishlistCardExtEntity>(readPoint, cancellationToken).ConfigureAwait(false);

        UserWishlistCardExtEntity existingRecord = _resolver.Resolve(readResponse, input);

        UserWishlistCardExtEntity updatedRecord = await _integrator.Integrate(existingRecord, input).ConfigureAwait(false);

        OpResponse<UserWishlistCardExtEntity> upsertResponse = await _userWishlistCardsScribe
            .UpsertAsync(updatedRecord, cancellationToken).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<UserWishlistCardExtEntity>(
                new UserWishlistCardsAdapterException($"Failed to add user wishlist card: {upsertResponse.StatusCode}"));
        }

        return new SuccessOperationResponse<UserWishlistCardExtEntity>(upsertResponse.Value);
    }
}
