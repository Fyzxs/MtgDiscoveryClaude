using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Actions.Mappers;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public sealed class UserWishlistCardsQueryMethods
{
    private readonly IEntryService _entryService;
    private readonly IGetUserWishlistArgsMapper _argsMapper;
    private readonly IOperationResponseToResponseModelMapper<List<CardItemOutEntity>> _responseMapper;

    public UserWishlistCardsQueryMethods(ILogger logger) : this(
        new EntryService(logger),
        new GetUserWishlistArgsMapper(),
        new OperationResponseToResponseModelMapper<List<CardItemOutEntity>>())
    {
    }

    private UserWishlistCardsQueryMethods(
        IEntryService entryService,
        IGetUserWishlistArgsMapper argsMapper,
        IOperationResponseToResponseModelMapper<List<CardItemOutEntity>> responseMapper)
    {
        _entryService = entryService;
        _argsMapper = argsMapper;
        _responseMapper = responseMapper;
    }

    [GraphQLType(typeof(CardResponseModelUnionType))]
    public async Task<ResponseModel> GetUserWishlistAsync(string userId)
    {
        IGetUserWishlistArgsEntity combinedArgs = await _argsMapper.Map(userId).ConfigureAwait(false);
        IOperationResponse<List<CardItemOutEntity>> response = await _entryService.GetUserWishlistAsync(combinedArgs).ConfigureAwait(false);
        return await _responseMapper.Map(response).ConfigureAwait(false);
    }
}
