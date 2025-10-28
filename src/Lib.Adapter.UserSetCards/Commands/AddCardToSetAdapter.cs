using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Commands.Integrators;
using Lib.Adapter.UserSetCards.Commands.Mappers;
using Lib.Adapter.UserSetCards.Commands.Resolvers;
using Lib.Adapter.UserSetCards.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
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

    public async Task<IOperationResponse<UserSetCardExtEntity>> Execute([NotNull] IAddCardToSetXfrEntity input)
    {
        ReadPointItem readPoint = await _readPointMapper.Map(input).ConfigureAwait(false);
        OpResponse<UserSetCardExtEntity> readResponse = await _userSetCardsGopher.ReadAsync<UserSetCardExtEntity>(readPoint).ConfigureAwait(false);

        UserSetCardExtEntity existingRecord = _resolver.Resolve(readResponse, input);
        UserSetCardExtEntity updatedRecord = _integrator.Integrate(existingRecord, input);

        OpResponse<UserSetCardExtEntity> upsertResponse = await _userSetCardsScribe.UpsertAsync(updatedRecord).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<UserSetCardExtEntity>(new UserSetCardsAdapterException());
        }

        return new SuccessOperationResponse<UserSetCardExtEntity>(upsertResponse.Value);
    }
}
