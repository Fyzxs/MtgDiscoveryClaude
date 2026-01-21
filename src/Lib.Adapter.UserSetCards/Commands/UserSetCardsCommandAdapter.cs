using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Adapter.UserSetCards.Apis;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Adapter.UserSetCards.Commands.Integrators;
using Lib.Adapter.UserSetCards.Commands.Mappers;
using Lib.Adapter.UserSetCards.Commands.Resolvers;
using Lib.Adapter.UserSetCards.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSetCards.Commands;

/// <summary>
/// Cosmos DB implementation of the user set cards command adapter.
///
/// This class handles all Cosmos DB-specific user set cards command operations,
/// implementing the specialized IUserSetCardsCommandAdapter interface.
/// </summary>
internal sealed class UserSetCardsCommandAdapter : IUserSetCardsCommandAdapter
{
    private readonly ICosmosScribe _userSetCardsScribe;
    private readonly ICosmosGopher _userSetCardsGopher;
    private readonly IAddCardToSetXfrToExtMapper _readPointMapper;
    private readonly IUserSetCardIntegrator _integrator;
    private readonly IUserSetCardResolver _resolver;
    private readonly IAddSetGroupXfrToReadPointMapper _setGroupMapper;
    private readonly IAddSetGroupResolver _setGroupResolver;
    private readonly IAddSetGroupIntegrator _setGroupIntegrator;

    public UserSetCardsCommandAdapter(ILogger logger) : this(
        new UserSetCardsScribe(logger),
        new UserSetCardsGopher(logger),
        new AddCardToSetXfrToExtMapper(),
        new UserSetCardIntegrator(),
        new UserSetCardResolver(),
        new AddSetGroupXfrToReadPointMapper(),
        new AddSetGroupResolver(),
        new AddSetGroupIntegrator())
    { }

    private UserSetCardsCommandAdapter(
        ICosmosScribe userSetCardsScribe,
        ICosmosGopher userSetCardsGopher,
        IAddCardToSetXfrToExtMapper readPointMapper,
        IUserSetCardIntegrator integrator,
        IUserSetCardResolver resolver,
        IAddSetGroupXfrToReadPointMapper setGroupMapper,
        IAddSetGroupResolver setGroupResolver,
        IAddSetGroupIntegrator setGroupIntegrator)
    {
        _userSetCardsScribe = userSetCardsScribe;
        _userSetCardsGopher = userSetCardsGopher;
        _readPointMapper = readPointMapper;
        _integrator = integrator;
        _resolver = resolver;
        _setGroupMapper = setGroupMapper;
        _setGroupResolver = setGroupResolver;
        _setGroupIntegrator = setGroupIntegrator;
    }

    public async Task<IOperationResponse<UserSetCardExtEntity>> AddCardToSetAsync(IAddCardToSetXfrEntity entity)
    {
        ReadPointItem readPoint = await _readPointMapper.Map(entity).ConfigureAwait(false);
        OpResponse<UserSetCardExtEntity> readResponse = await _userSetCardsGopher.ReadAsync<UserSetCardExtEntity>(readPoint).ConfigureAwait(false);

        UserSetCardExtEntity existingRecord = _resolver.Resolve(readResponse, entity);
        UserSetCardExtEntity updatedRecord = _integrator.Integrate(existingRecord, entity);

        OpResponse<UserSetCardExtEntity> upsertResponse = await _userSetCardsScribe.UpsertAsync(updatedRecord).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<UserSetCardExtEntity>(new UserSetCardsAdapterException());
        }

        return new SuccessOperationResponse<UserSetCardExtEntity>(upsertResponse.Value);
    }

    public async Task<IOperationResponse<UserSetCardExtEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardXfrEntity addSetGroup)
    {
        ReadPointItem readPoint = await _setGroupMapper.Map(addSetGroup).ConfigureAwait(false);
        OpResponse<UserSetCardExtEntity> readResponse = await _userSetCardsGopher.ReadAsync<UserSetCardExtEntity>(readPoint).ConfigureAwait(false);

        UserSetCardExtEntity existingRecord = _setGroupResolver.Resolve(readResponse, addSetGroup);
        UserSetCardExtEntity updatedRecord = _setGroupIntegrator.Integrate(existingRecord, addSetGroup);

        OpResponse<UserSetCardExtEntity> upsertResponse = await _userSetCardsScribe.UpsertAsync(updatedRecord).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful()) return new FailureOperationResponse<UserSetCardExtEntity>(new UserSetCardsAdapterException());

        return new SuccessOperationResponse<UserSetCardExtEntity>(upsertResponse.Value);
    }
}
