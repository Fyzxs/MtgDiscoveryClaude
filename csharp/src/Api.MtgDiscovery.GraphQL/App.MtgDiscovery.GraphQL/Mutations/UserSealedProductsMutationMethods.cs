using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Actions.Mappers;
using App.MtgDiscovery.GraphQL.Entities.Args.UserSealedProducts;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Mutations;

[ExtendObjectType(typeof(ApiMutation))]
internal sealed class UserSealedProductsMutationMethods
{
    private readonly IEntryService _entryService;
    private readonly IAddUserSealedProductArgsMapper _argsMapper;
    private readonly IOperationResponseToResponseModelMapper<List<SealedProductOutEntity>> _responseMapper;

    public UserSealedProductsMutationMethods(ILogger logger) : this(
        new EntryService(logger),
        new AddUserSealedProductArgsMapper(),
        new OperationResponseToResponseModelMapper<List<SealedProductOutEntity>>())
    {
    }

    private UserSealedProductsMutationMethods(
        IEntryService entryService,
        IAddUserSealedProductArgsMapper argsMapper,
        IOperationResponseToResponseModelMapper<List<SealedProductOutEntity>> responseMapper)
    {
        _entryService = entryService;
        _argsMapper = argsMapper;
        _responseMapper = responseMapper;
    }

    [Authorize]
    [GraphQLType(typeof(AddUserSealedProductResponseModelUnionType))]
    public async Task<ResponseModel> AddUserSealedProductAsync(ClaimsPrincipal claimsPrincipal, AddUserSealedProductArgEntity args, CancellationToken cancellationToken)
    {
        IAddSealedProductToCollectionArgsEntity combinedArgs = await _argsMapper.Map(claimsPrincipal, args).ConfigureAwait(false);
        IOperationResponse<List<SealedProductOutEntity>> response = await _entryService.AddSealedProductToCollectionAsync(combinedArgs, cancellationToken).ConfigureAwait(false);
        return await _responseMapper.Map(response).ConfigureAwait(false);
    }
}
