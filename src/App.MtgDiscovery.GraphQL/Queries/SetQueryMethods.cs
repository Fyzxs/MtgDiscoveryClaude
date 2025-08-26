using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args;
using App.MtgDiscovery.GraphQL.Entities.Outs.Sets;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Mappers;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public class SetQueryMethods
{
    private readonly IScryfallSetMapper _scryfallSetMapper;
    private readonly IEntryService _entryService;

    public SetQueryMethods(ILogger logger) : this(new ScryfallSetMapper(), new EntryService(logger))
    {
    }

    private SetQueryMethods(IScryfallSetMapper scryfallSetMapper, IEntryService entryService)
    {
        _scryfallSetMapper = scryfallSetMapper;
        _entryService = entryService;
    }

    public string TestSet() => "Set query endpoint is working!";

    [GraphQLType(typeof(SetResponseModelUnionType))]
    public async Task<ResponseModel> SetsById(SetIdsArgEntity ids)
    {
        IOperationResponse<ISetItemCollectionItrEntity> response = await _entryService.SetsByIdsAsync(ids).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        List<ScryfallSetOutEntity> results = [];

        foreach (ISetItemItrEntity setItem in response.ResponseData.Data)
        {
            ScryfallSetOutEntity outEntity = _scryfallSetMapper.Map(setItem);
            results.Add(outEntity);
        }

        return new SuccessDataResponseModel<List<ScryfallSetOutEntity>>() { Data = results };
    }

    [GraphQLType(typeof(SetResponseModelUnionType))]
    public async Task<ResponseModel> SetsByCode(SetCodesArgEntity codes)
    {
        IOperationResponse<ISetItemCollectionItrEntity> response = await _entryService.SetsByCodeAsync(codes).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        List<ScryfallSetOutEntity> results = [];

        foreach (ISetItemItrEntity setItem in response.ResponseData.Data)
        {
            ScryfallSetOutEntity outEntity = _scryfallSetMapper.Map(setItem);
            results.Add(outEntity);
        }

        return new SuccessDataResponseModel<List<ScryfallSetOutEntity>>() { Data = results };
    }

    [GraphQLType(typeof(SetResponseModelUnionType))]
    public async Task<ResponseModel> AllSets()
    {
        IOperationResponse<ISetItemCollectionItrEntity> response = await _entryService.AllSetsAsync().ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        List<ScryfallSetOutEntity> results = [];

        foreach (ISetItemItrEntity setItem in response.ResponseData.Data)
        {
            ScryfallSetOutEntity outEntity = _scryfallSetMapper.Map(setItem);
            results.Add(outEntity);
        }

        return new SuccessDataResponseModel<List<ScryfallSetOutEntity>>() { Data = results };
    }
}
