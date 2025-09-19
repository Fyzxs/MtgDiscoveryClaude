using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate.Types;
using HotChocolate;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public class CardQueryMethods
{
    private readonly IEntryService _entryService;

    public CardQueryMethods(ILogger logger) : this(new EntryService(logger))
    {
    }

    private CardQueryMethods(IEntryService entryService)
    {
        _entryService = entryService;
    }

    public string Test() => "Card query endpoint is working!";

    [GraphQLType(typeof(CardResponseModelUnionType))]
    public async Task<ResponseModel> CardsById(CardIdsArgEntity ids)
    {
        IOperationResponse<List<CardItemOutEntity>> response = await _entryService.CardsByIdsAsync(ids).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        return new SuccessDataResponseModel<List<CardItemOutEntity>>() { Data = response.ResponseData };
    }

    [GraphQLType(typeof(CardResponseModelUnionType))]
    public async Task<ResponseModel> CardsBySetCode(SetCodeArgEntity setCode)
    {
        IOperationResponse<List<CardItemOutEntity>> response = await _entryService.CardsBySetCodeAsync(setCode).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        return new SuccessDataResponseModel<List<CardItemOutEntity>>() { Data = response.ResponseData };
    }

    [GraphQLType(typeof(CardResponseModelUnionType))]
    public async Task<ResponseModel> CardsByName(CardNameArgEntity cardName)
    {
        IOperationResponse<List<CardItemOutEntity>> response = await _entryService.CardsByNameAsync(cardName).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        return new SuccessDataResponseModel<List<CardItemOutEntity>>() { Data = response.ResponseData };
    }

    [GraphQLType(typeof(CardNameSearchResponseModelUnionType))]
    public async Task<ResponseModel> CardNameSearch(CardSearchTermArgEntity searchTerm)
    {
        IOperationResponse<List<CardNameSearchResultOutEntity>> response = await _entryService.CardNameSearchAsync(searchTerm).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        return new SuccessDataResponseModel<List<CardNameSearchResultOutEntity>>() { Data = response.ResponseData };
    }
}
