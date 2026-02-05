using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserCards.Commands.Integrators;
using Lib.Adapter.UserCards.Commands.Mappers;
using Lib.Adapter.UserCards.Commands.Resolvers;
using Lib.Adapter.UserCards.Commands.Strategies;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Commands;

/// <summary>
/// Adds or updates a card in a user's collection with intelligent merging logic.
/// </summary>
internal sealed class AddUserCardAdapter : IAddUserCardAdapter
{
    private readonly ICosmosGopher _userCardsGopher;
    private readonly ICosmosScribe _userCardsScribe;
    private readonly IAddUserCardXfrToReadPointMapper _readPointMapper;
    private readonly IUserCardResolver _resolver;
    private readonly IUserCardIntegrator _integrator;
    private readonly ICosmosRetryStrategy _retryStrategy;

    public AddUserCardAdapter(ILogger logger) : this(
        new UserCardsGopher(logger),
        new UserCardsScribe(logger),
        new AddUserCardXfrToReadPointMapper(),
        new UserCardResolver(),
        new UserCardIntegrator(),
        new CosmosRetryStrategy())
    { }

    private AddUserCardAdapter(
        ICosmosGopher userCardsGopher,
        ICosmosScribe userCardsScribe,
        IAddUserCardXfrToReadPointMapper readPointMapper,
        IUserCardResolver resolver,
        IUserCardIntegrator integrator,
        ICosmosRetryStrategy retryStrategy)
    {
        _userCardsGopher = userCardsGopher;
        _userCardsScribe = userCardsScribe;
        _readPointMapper = readPointMapper;
        _resolver = resolver;
        _integrator = integrator;
        _retryStrategy = retryStrategy;
    }

    public async Task<IOperationResponse<UserCardExtEntity>> Execute(
        [NotNull] IAddUserCardXfrEntity input,
        CancellationToken cancellationToken)
    {
        return await _retryStrategy.ExecuteWithRetry<UserCardExtEntity>(async () =>
        {
            // Step 1: Try to read existing record
            ReadPointItem readPoint = await _readPointMapper.Map(input).ConfigureAwait(false);
            OpResponse<UserCardExtEntity> readResponse = await _userCardsGopher.ReadAsync<UserCardExtEntity>(readPoint).ConfigureAwait(false);

            // Step 2: Resolve existing record or create new one
            UserCardExtEntity existingRecord = _resolver.Resolve(readResponse, input);

            // Step 3: Integrate changes (merge or replace based on ReplaceMode)
            UserCardExtEntity updatedRecord = await _integrator.Integrate(existingRecord, input).ConfigureAwait(false);

            // Step 4: Upsert the merged/new item
            OpResponse<UserCardExtEntity> upsertResponse = await _userCardsScribe.UpsertAsync(updatedRecord).ConfigureAwait(false);

            if (upsertResponse.IsNotSuccessful())
            {
                return new FailureOperationResponse<UserCardExtEntity>(
                    new UserCardsAdapterException($"Failed to add user card: {upsertResponse.StatusCode}"));
            }

            // Step 5: Return the raw ExtEntity result
            return new SuccessOperationResponse<UserCardExtEntity>(upsertResponse.Value);
        }).ConfigureAwait(false);
    }
}
