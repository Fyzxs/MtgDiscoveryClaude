using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Mappers;
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
public sealed class UserCardsMutationMethods
{
    private readonly IEntryService _entryService;
    private readonly IAddCardToCollectionArgsMapper _argsMapper;
    private readonly IOperationResponseToResponseModelMapper<List<CardItemOutEntity>> _cardResponseMapper;

    public UserCardsMutationMethods(ILogger logger) : this(
        new EntryService(logger),
        new AddCardToCollectionArgsMapper(),
        new OperationResponseToResponseModelMapper<List<CardItemOutEntity>>())
    {
    }

    private UserCardsMutationMethods(
        IEntryService entryService,
        IAddCardToCollectionArgsMapper argsMapper,
        IOperationResponseToResponseModelMapper<List<CardItemOutEntity>> cardResponseMapper)
    {
        _entryService = entryService;
        _argsMapper = argsMapper;
        _cardResponseMapper = cardResponseMapper;
    }

    [Authorize]
    [GraphQLType(typeof(AddCardToCollectionResponseModelUnionType))]
    public async Task<ResponseModel> AddCardToCollectionAsync(ClaimsPrincipal claimsPrincipal, AddUserCardArgEntity args)
    {
        IAddCardToCollectionArgsEntity combinedArgs = await _argsMapper.Map(claimsPrincipal, args).ConfigureAwait(false);
        IOperationResponse<List<CardItemOutEntity>> response = await _entryService.AddCardToCollectionAsync(combinedArgs).ConfigureAwait(false);
        return await _cardResponseMapper.Map(response).ConfigureAwait(false);
    }
}
