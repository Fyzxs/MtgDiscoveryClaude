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
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserSetCards.Commands;

/// <summary>
/// Adds a set group to a user's set collection with atomic read-modify-write.
/// </summary>
internal sealed class AddSetGroupToUserSetCardAdapter : IAddSetGroupToUserSetCardAdapter
{
    private readonly ICosmosScribe _userSetCardsScribe;
    private readonly ICosmosGopher _userSetCardsGopher;
    private readonly IAddSetGroupXfrToReadPointMapper _setGroupMapper;
    private readonly IAddSetGroupResolver _setGroupResolver;
    private readonly IAddSetGroupIntegrator _setGroupIntegrator;

    public AddSetGroupToUserSetCardAdapter(ILogger logger) : this(new UserSetCardsScribe(logger), new UserSetCardsGopher(logger), new AddSetGroupXfrToReadPointMapper(), new AddSetGroupResolver(), new AddSetGroupIntegrator()) { }

    private AddSetGroupToUserSetCardAdapter(ICosmosScribe userSetCardsScribe, ICosmosGopher userSetCardsGopher, IAddSetGroupXfrToReadPointMapper setGroupMapper, IAddSetGroupResolver setGroupResolver, IAddSetGroupIntegrator setGroupIntegrator)
    {
        _userSetCardsScribe = userSetCardsScribe;
        _userSetCardsGopher = userSetCardsGopher;
        _setGroupMapper = setGroupMapper;
        _setGroupResolver = setGroupResolver;
        _setGroupIntegrator = setGroupIntegrator;
    }

    public async Task<IOperationResponse<UserSetCardExtEntity>> Execute([NotNull] IAddSetGroupToUserSetCardXfrEntity input)
    {
        ReadPointItem readPoint = await _setGroupMapper.Map(input).ConfigureAwait(false);
        OpResponse<UserSetCardExtEntity> readResponse = await _userSetCardsGopher.ReadAsync<UserSetCardExtEntity>(readPoint).ConfigureAwait(false);

        UserSetCardExtEntity existingRecord = _setGroupResolver.Resolve(readResponse, input);
        UserSetCardExtEntity updatedRecord = _setGroupIntegrator.Integrate(existingRecord, input);

        OpResponse<UserSetCardExtEntity> upsertResponse = await _userSetCardsScribe.UpsertAsync(updatedRecord).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful()) return new FailureOperationResponse<UserSetCardExtEntity>(new UserSetCardsAdapterException());

        return new SuccessOperationResponse<UserSetCardExtEntity>(upsertResponse.Value);
    }
}
