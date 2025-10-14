// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Mappers;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public class SetQueryMethods
{
    private readonly IEntryService _entryService;
    private readonly IOperationResponseToResponseModelMapper<List<ScryfallSetOutEntity>> _setResponseMapper;

    public SetQueryMethods(ILogger logger) : this(
        new EntryService(logger),
        new OperationResponseToResponseModelMapper<List<ScryfallSetOutEntity>>())
    {
    }

    private SetQueryMethods(
        IEntryService entryService,
        IOperationResponseToResponseModelMapper<List<ScryfallSetOutEntity>> setResponseMapper)
    {
        _entryService = entryService;
        _setResponseMapper = setResponseMapper;
    }

    public string TestSet() => "Set query endpoint is working!";

    [GraphQLType(typeof(SetResponseModelUnionType))]
    public async Task<ResponseModel> SetsById(SetIdsArgEntity ids)
    {
        IOperationResponse<List<ScryfallSetOutEntity>> response = await _entryService.SetsByIdsAsync(ids).ConfigureAwait(false);
        return await _setResponseMapper.Map(response).ConfigureAwait(false);
    }

    [GraphQLType(typeof(SetResponseModelUnionType))]
    public async Task<ResponseModel> SetsByCode(SetCodesArgEntity codes)
    {
        IOperationResponse<List<ScryfallSetOutEntity>> response = await _entryService.SetsByCodeAsync(codes).ConfigureAwait(false);
        return await _setResponseMapper.Map(response).ConfigureAwait(false);
    }

    [GraphQLType(typeof(SetResponseModelUnionType))]
    public async Task<ResponseModel> AllSets()
    {
        IOperationResponse<List<ScryfallSetOutEntity>> response = await _entryService.AllSetsAsync().ConfigureAwait(false);
        return await _setResponseMapper.Map(response).ConfigureAwait(false);
    }
}
