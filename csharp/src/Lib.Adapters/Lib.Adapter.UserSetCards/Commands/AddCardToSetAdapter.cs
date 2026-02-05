using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSetCards;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Commands.Integrators;
using Lib.Adapter.UserSetCards.Commands.Mappers;
using Lib.Adapter.UserSetCards.Commands.Resolvers;
using Lib.Adapter.UserSetCards.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSetCards.Commands;

/// <summary>
/// Adds or removes a card from a user's set collection with atomic read-modify-write.
/// </summary>
internal sealed class AddCardToSetAdapter : IAddCardToSetAdapter
{
    private readonly ICosmosScribe _userSetCardsScribe;
    private readonly ICosmosGopher _userSetCardsGopher;
    private readonly IAddCardToSetXfrToExtMapper _readPointMapper;
    private readonly IUserSetCardIntegrator _integrator;
    private readonly IUserSetCardResolver _resolver;

    public AddCardToSetAdapter(ILogger logger) : this(new UserSetCardsScribe(logger), new UserSetCardsGopher(logger), new AddCardToSetXfrToExtMapper(), new UserSetCardIntegrator(), new UserSetCardResolver()) { }

    private AddCardToSetAdapter(ICosmosScribe userSetCardsScribe, ICosmosGopher userSetCardsGopher, IAddCardToSetXfrToExtMapper readPointMapper, IUserSetCardIntegrator integrator, IUserSetCardResolver resolver)
    {
        _userSetCardsScribe = userSetCardsScribe;
        _userSetCardsGopher = userSetCardsGopher;
        _readPointMapper = readPointMapper;
        _integrator = integrator;
        _resolver = resolver;
    }

    public async Task<IOperationResponse<UserSetCardExtEntity>> Execute(
        [NotNull] IAddCardToSetXfrEntity input,
        CancellationToken cancellationToken)
    {
        const int MaxRetries = 5;
        int retryCount = 0;

        while (retryCount < MaxRetries)
        {
            try
            {
                ReadPointItem readPoint = await _readPointMapper.Map(input).ConfigureAwait(false);
                OpResponse<UserSetCardExtEntity> readResponse = await _userSetCardsGopher.ReadAsync<UserSetCardExtEntity>(readPoint, cancellationToken).ConfigureAwait(false);

                UserSetCardExtEntity existingRecord = _resolver.Resolve(readResponse, input);
                UserSetCardExtEntity updatedRecord = await _integrator.Integrate(existingRecord, input).ConfigureAwait(false);

                OpResponse<UserSetCardExtEntity> upsertResponse = await _userSetCardsScribe.UpsertAsync(updatedRecord, cancellationToken).ConfigureAwait(false);

                if (upsertResponse.IsNotSuccessful())
                {
                    return new FailureOperationResponse<UserSetCardExtEntity>(new UserSetCardsAdapterException("Failed to upsert user set card"));
                }

                return new SuccessOperationResponse<UserSetCardExtEntity>(upsertResponse.Value);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.PreconditionFailed && retryCount < MaxRetries)
            {
                // ETag mismatch detected - another request modified the document
                retryCount++;

                if (retryCount >= MaxRetries)
                {
                    return new FailureOperationResponse<UserSetCardExtEntity>(
                        new UserSetCardsAdapterException($"Failed to add card to set after {MaxRetries} retries due to concurrent updates", ex));
                }

                // Exponential backoff: 50ms, 100ms, 200ms, 400ms, 800ms
                int delayMs = 50 * (1 << (retryCount - 1));
                await Task.Delay(delayMs, cancellationToken).ConfigureAwait(false);

                // Loop will retry with fresh read
            }
        }

        // Should never reach here due to loop logic, but satisfy compiler
        return new FailureOperationResponse<UserSetCardExtEntity>(new UserSetCardsAdapterException("Unexpected termination of retry loop"));
    }
}
