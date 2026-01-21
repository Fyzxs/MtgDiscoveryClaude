using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args;
using App.MtgDiscovery.GraphQL.Entities.Outs.Cards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Mappers;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public class CardQueryMethods
{
    private readonly ICollectionCardItemItrToOutMapper _cardCollectionMapper;
    private readonly IEntryService _entryService;

    public CardQueryMethods(ILogger logger) : this(new CollectionCardItemItrToOutMapper(), new EntryService(logger))
    {
    }

    private CardQueryMethods(ICollectionCardItemItrToOutMapper cardCollectionMapper, IEntryService entryService)
    {
        _cardCollectionMapper = cardCollectionMapper;
        _entryService = entryService;
    }

    public string Test() => "Card query endpoint is working!";

    [GraphQLType(typeof(CardResponseModelUnionType))]
    public async Task<ResponseModel> CardsById(CardIdsArgEntity ids)
    {
        IOperationResponse<ICardItemCollectionItrEntity> response = await _entryService.CardsByIdsAsync(ids).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        ICollection<CardItemOutEntity> results = await _cardCollectionMapper.Map(response.ResponseData.Data).ConfigureAwait(false);

        return new SuccessDataResponseModel<ICollection<CardItemOutEntity>>() { Data = results };
    }

    [GraphQLType(typeof(CardResponseModelUnionType))]
    public async Task<ResponseModel> CardsBySetCode(SetCodeArgEntity setCode)
    {
        IOperationResponse<ICardItemCollectionItrEntity> response = await _entryService.CardsBySetCodeAsync(setCode).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        ICollection<CardItemOutEntity> results = await _cardCollectionMapper.Map(response.ResponseData.Data).ConfigureAwait(false);

        return new SuccessDataResponseModel<ICollection<CardItemOutEntity>>() { Data = results };
    }

    [GraphQLType(typeof(CardResponseModelUnionType))]
    public async Task<ResponseModel> CardsByName(CardNameArgEntity cardName)
    {
        IOperationResponse<ICardItemCollectionItrEntity> response = await _entryService.CardsByNameAsync(cardName).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        ICollection<CardItemOutEntity> results = await _cardCollectionMapper.Map(response.ResponseData.Data).ConfigureAwait(false);

        return new SuccessDataResponseModel<ICollection<CardItemOutEntity>>() { Data = results };
    }

    [GraphQLType(typeof(CardNameSearchResponseModelUnionType))]
    public async Task<ResponseModel> CardNameSearch(CardSearchTermArgEntity searchTerm)
    {
        IOperationResponse<ICardNameSearchResultCollectionItrEntity> response = await _entryService.CardNameSearchAsync(searchTerm).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        List<CardNameSearchResultOutEntity> results = [];

        foreach (ICardNameSearchResultItrEntity nameResult in response.ResponseData.Names)
        {
            results.Add(new CardNameSearchResultOutEntity { Name = nameResult.Name });
        }

        return new SuccessDataResponseModel<ICollection<CardNameSearchResultOutEntity>>() { Data = results };
    }
}
