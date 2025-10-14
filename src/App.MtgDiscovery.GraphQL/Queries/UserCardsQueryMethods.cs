using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Mappers;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public sealed class UserCardsQueryMethods
{
    private readonly IEntryService _entryService;
    private readonly IOperationResponseToResponseModelMapper<List<UserCardOutEntity>> _userCardResponseMapper;

    public UserCardsQueryMethods(ILogger logger) : this(
        new EntryService(logger),
        new OperationResponseToResponseModelMapper<List<UserCardOutEntity>>())
    {
    }

    private UserCardsQueryMethods(
        IEntryService entryService,
        IOperationResponseToResponseModelMapper<List<UserCardOutEntity>> userCardResponseMapper)
    {
        _entryService = entryService;
        _userCardResponseMapper = userCardResponseMapper;
    }

    [GraphQLType(typeof(UserCardsCollectionResponseModelUnionType))]
    public async Task<ResponseModel> UserCardsBySet(UserCardsBySetArgEntity setArgs)
    {
        IOperationResponse<List<UserCardOutEntity>> response = await _entryService
            .UserCardsBySetAsync(setArgs)
            .ConfigureAwait(false);
        return await _userCardResponseMapper.Map(response).ConfigureAwait(false);
    }

    [GraphQLType(typeof(UserCardsCollectionResponseModelUnionType))]
    public async Task<ResponseModel> UserCard(UserCardArgEntity cardArgs)
    {
        IOperationResponse<List<UserCardOutEntity>> response = await _entryService
            .UserCardAsync(cardArgs)
            .ConfigureAwait(false);
        return await _userCardResponseMapper.Map(response).ConfigureAwait(false);
    }

    [GraphQLType(typeof(UserCardsCollectionResponseModelUnionType))]
    public async Task<ResponseModel> UserCardsByIds(UserCardsByIdsArgEntity cardsArgs)
    {
        IOperationResponse<List<UserCardOutEntity>> response = await _entryService
            .UserCardsByIdsAsync(cardsArgs)
            .ConfigureAwait(false);
        return await _userCardResponseMapper.Map(response).ConfigureAwait(false);
    }
}
