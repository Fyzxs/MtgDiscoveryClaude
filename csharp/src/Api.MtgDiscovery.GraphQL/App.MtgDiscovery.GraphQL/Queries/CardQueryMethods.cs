using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Actions.Mappers;
using App.MtgDiscovery.GraphQL.Entities.Args;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
internal sealed class CardQueryMethods
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
    public async Task<ResponseModel> CardsById(
        CardIdsArgEntity ids,
        CancellationToken cancellationToken)
    {
        IOperationResponse<List<CardItemOutEntity>> response = await _entryService.CardsByIdsAsync(ids, cancellationToken).ConfigureAwait(false);
        return await _cardResponseMapper.Map(response).ConfigureAwait(false);
    }

    [GraphQLType(typeof(CardResponseModelUnionType))]
    public async Task<ResponseModel> CardsBySetCode(
        SetCodeArgEntity setCode,
        CancellationToken cancellationToken)
    {
        IOperationResponse<List<CardItemOutEntity>> response = await _entryService.CardsBySetCodeAsync(setCode, cancellationToken).ConfigureAwait(false);
        return await _cardResponseMapper.Map(response).ConfigureAwait(false);
    }

    [GraphQLType(typeof(CardResponseModelUnionType))]
    public async Task<ResponseModel> CardsByName(
        CardNameArgEntity cardName,
        CancellationToken cancellationToken)
    {
        IOperationResponse<List<CardItemOutEntity>> response = await _entryService.CardsByNameAsync(cardName, cancellationToken).ConfigureAwait(false);
        return await _cardResponseMapper.Map(response).ConfigureAwait(false);
    }

    [GraphQLType(typeof(CardNameSearchResponseModelUnionType))]
    public async Task<ResponseModel> CardNameSearch(
        CardSearchTermArgEntity searchTerm,
        CancellationToken cancellationToken)
    {
        IOperationResponse<List<CardNameSearchResultOutEntity>> response = await _entryService.CardNameSearchAsync(searchTerm, cancellationToken).ConfigureAwait(false);
        return await _cardNameSearchResponseMapper.Map(response).ConfigureAwait(false);
    }
}
