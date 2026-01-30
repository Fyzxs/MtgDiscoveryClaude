using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Actions.Mappers;
using App.MtgDiscovery.GraphQL.Entities.Args.UserWishlistCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Mutations;

[ExtendObjectType(typeof(ApiMutation))]
public sealed class UserWishlistCardsMutationMethods
{
    private readonly IEntryService _entryService;
    private readonly IAddCardToWishlistArgsMapper _addArgsMapper;
    private readonly IOperationResponseToResponseModelMapper<List<CardItemOutEntity>> _cardResponseMapper;

    public UserWishlistCardsMutationMethods(ILogger logger) : this(
        new EntryService(logger),
        new AddCardToWishlistArgsMapper(),
        new OperationResponseToResponseModelMapper<List<CardItemOutEntity>>())
    {
    }

    private UserWishlistCardsMutationMethods(
        IEntryService entryService,
        IAddCardToWishlistArgsMapper addArgsMapper,
        IOperationResponseToResponseModelMapper<List<CardItemOutEntity>> cardResponseMapper)
    {
        _entryService = entryService;
        _addArgsMapper = addArgsMapper;
        _cardResponseMapper = cardResponseMapper;
    }

    [Authorize]
    [GraphQLType(typeof(AddCardToWishlistResponseModelUnionType))]
    public async Task<ResponseModel> AddCardToWishlistAsync(ClaimsPrincipal claimsPrincipal, AddUserWishlistCardArgEntity args)
    {
        IAddCardToWishlistArgsEntity combinedArgs = await _addArgsMapper.Map(claimsPrincipal, args).ConfigureAwait(false);
        IOperationResponse<List<CardItemOutEntity>> response = await _entryService.AddCardToWishlistAsync(combinedArgs).ConfigureAwait(false);
        return await _cardResponseMapper.Map(response).ConfigureAwait(false);
    }
}
