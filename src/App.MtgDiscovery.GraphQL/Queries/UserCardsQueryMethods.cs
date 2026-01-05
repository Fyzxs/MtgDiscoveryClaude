using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Actions.Mappers;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Signing;
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
    private readonly IOperationResponseToResponseModelMapper<SigningResultOutEntity> _signingResultResponseMapper;

    public UserCardsQueryMethods(ILogger logger) : this(
        new EntryService(logger),
        new OperationResponseToResponseModelMapper<List<UserCardOutEntity>>(),
        new OperationResponseToResponseModelMapper<SigningResultOutEntity>())
    {
    }

    private UserCardsQueryMethods(
        IEntryService entryService,
        IOperationResponseToResponseModelMapper<List<UserCardOutEntity>> userCardResponseMapper,
        IOperationResponseToResponseModelMapper<SigningResultOutEntity> signingResultResponseMapper)
    {
        _entryService = entryService;
        _userCardResponseMapper = userCardResponseMapper;
        _signingResultResponseMapper = signingResultResponseMapper;
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

    [GraphQLType(typeof(SigningResultResponseModelUnionType))]
    public async Task<ResponseModel> UserCardsForSigning(UserCardsForSigningArgEntity forSigningArgs)
    {
        IOperationResponse<SigningResultOutEntity> response = await _entryService
            .UserCardsForSigningAsync(forSigningArgs)
            .ConfigureAwait(false);
        return await _signingResultResponseMapper.Map(response).ConfigureAwait(false);
    }
}
