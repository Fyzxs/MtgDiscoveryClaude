// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public class SetQueryMethods
{
    private readonly ISetItemCollectionItrToOutMapper _setCollectionMapper;
    private readonly IEntryService _entryService;

    public SetQueryMethods(ILogger logger) : this(new SetItemCollectionItrToOutMapper(), new EntryService(logger))
    {
    }

    private SetQueryMethods(ISetItemCollectionItrToOutMapper setCollectionMapper, IEntryService entryService)
    {
        _setCollectionMapper = setCollectionMapper;
        _entryService = entryService;
    }

    public string TestSet() => "Set query endpoint is working!";

    [GraphQLType(typeof(SetResponseModelUnionType))]
    public async Task<ResponseModel> SetsById(SetIdsArgEntity ids)
    {
        IOperationResponse<ISetItemCollectionItrEntity> response = await _entryService.SetsByIdsAsync(ids).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureResponseModel()
            {
                Status = new StatusDataModel()
                {
                    Message = response.OuterException.StatusMessage,
                    StatusCode = response.OuterException.StatusCode
                }
            };
        }

        List<ScryfallSetOutEntity> results = await _setCollectionMapper.Map(response.ResponseData.Data).ConfigureAwait(false);

        return new SuccessDataResponseModel<List<ScryfallSetOutEntity>>() { Data = results };
    }

    [GraphQLType(typeof(SetResponseModelUnionType))]
    public async Task<ResponseModel> SetsByCode(SetCodesArgEntity codes)
    {
        IOperationResponse<ISetItemCollectionItrEntity> response = await _entryService.SetsByCodeAsync(codes).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureResponseModel()
            {
                Status = new StatusDataModel()
                {
                    Message = response.OuterException.StatusMessage,
                    StatusCode = response.OuterException.StatusCode
                }
            };
        }

        List<ScryfallSetOutEntity> results = await _setCollectionMapper.Map(response.ResponseData.Data).ConfigureAwait(false);

        return new SuccessDataResponseModel<List<ScryfallSetOutEntity>>() { Data = results };
    }

    [GraphQLType(typeof(SetResponseModelUnionType))]
    public async Task<ResponseModel> AllSets()
    {
        IOperationResponse<ISetItemCollectionItrEntity> response = await _entryService.AllSetsAsync().ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureResponseModel()
            {
                Status = new StatusDataModel()
                {
                    Message = response.OuterException.StatusMessage,
                    StatusCode = response.OuterException.StatusCode
                }
            };
        }

        List<ScryfallSetOutEntity> results = await _setCollectionMapper.Map(response.ResponseData.Data).ConfigureAwait(false);

        return new SuccessDataResponseModel<List<ScryfallSetOutEntity>>() { Data = results };
    }
}
