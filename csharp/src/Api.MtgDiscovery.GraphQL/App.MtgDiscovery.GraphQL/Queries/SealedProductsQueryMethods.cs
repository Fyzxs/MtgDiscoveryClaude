using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Actions.Mappers;
using App.MtgDiscovery.GraphQL.Entities.Args.SealedProducts;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
internal sealed class SealedProductsQueryMethods
{
    private readonly IEntryService _entryService;
    private readonly IOperationResponseToResponseModelMapper<List<SealedProductOutEntity>> _responseMapper;

    public SealedProductsQueryMethods(ILogger logger) : this(
        new EntryService(logger),
        new OperationResponseToResponseModelMapper<List<SealedProductOutEntity>>())
    {
    }

    private SealedProductsQueryMethods(
        IEntryService entryService,
        IOperationResponseToResponseModelMapper<List<SealedProductOutEntity>> responseMapper)
    {
        _entryService = entryService;
        _responseMapper = responseMapper;
    }

    [GraphQLType(typeof(SealedProductsResponseModelUnionType))]
    public async Task<ResponseModel> SealedProductsBySetCode(
        GetSealedProductsBySetCodeArgEntity args,
        CancellationToken cancellationToken)
    {
        IOperationResponse<List<SealedProductOutEntity>> response = await _entryService
            .SealedProductsBySetCodeAsync(args, cancellationToken)
            .ConfigureAwait(false);

        return await _responseMapper.Map(response).ConfigureAwait(false);
    }
}
