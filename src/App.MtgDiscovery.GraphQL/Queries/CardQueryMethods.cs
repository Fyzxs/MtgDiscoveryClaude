using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Mappers;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public class CardQueryMethods
{
    private readonly IEntryService _entryService;
    private readonly IOperationResponseToResponseModelMapper<List<CardItemOutEntity>> _cardResponseMapper;
    private readonly IOperationResponseToResponseModelMapper<List<CardNameSearchResultOutEntity>> _cardNameSearchResponseMapper;

    public CardQueryMethods(ILogger logger) : this(
        new EntryService(logger),
        new OperationResponseToResponseModelMapper<List<CardItemOutEntity>>(),
        new OperationResponseToResponseModelMapper<List<CardNameSearchResultOutEntity>>())
    {
    }

    private CardQueryMethods(
        IEntryService entryService,
        IOperationResponseToResponseModelMapper<List<CardItemOutEntity>> cardResponseMapper,
        IOperationResponseToResponseModelMapper<List<CardNameSearchResultOutEntity>> cardNameSearchResponseMapper)
    {
        _entryService = entryService;
        _cardResponseMapper = cardResponseMapper;
        _cardNameSearchResponseMapper = cardNameSearchResponseMapper;
    }

    public string Test() => "Card query endpoint is working!";

    [GraphQLType(typeof(CardResponseModelUnionType))]
    public async Task<ResponseModel> CardsById(CardIdsArgEntity ids)
    {
        IOperationResponse<List<CardItemOutEntity>> response = await _entryService.CardsByIdsAsync(ids).ConfigureAwait(false);
        return await _cardResponseMapper.Map(response).ConfigureAwait(false);
    }

    [GraphQLType(typeof(CardResponseModelUnionType))]
    public async Task<ResponseModel> CardsBySetCode(SetCodeArgEntity setCode)
    {
        IOperationResponse<List<CardItemOutEntity>> response = await _entryService.CardsBySetCodeAsync(setCode).ConfigureAwait(false);
        return await _cardResponseMapper.Map(response).ConfigureAwait(false);
    }

    [GraphQLType(typeof(CardResponseModelUnionType))]
    public async Task<ResponseModel> CardsByName(CardNameArgEntity cardName)
    {
        IOperationResponse<List<CardItemOutEntity>> response = await _entryService.CardsByNameAsync(cardName).ConfigureAwait(false);
        return await _cardResponseMapper.Map(response).ConfigureAwait(false);
    }

    [GraphQLType(typeof(CardNameSearchResponseModelUnionType))]
    public async Task<ResponseModel> CardNameSearch(CardSearchTermArgEntity searchTerm)
    {
        IOperationResponse<List<CardNameSearchResultOutEntity>> response = await _entryService.CardNameSearchAsync(searchTerm).ConfigureAwait(false);
        return await _cardNameSearchResponseMapper.Map(response).ConfigureAwait(false);
    }
}
