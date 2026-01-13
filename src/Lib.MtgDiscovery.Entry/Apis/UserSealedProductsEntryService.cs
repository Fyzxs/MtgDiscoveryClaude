using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.UserSealedProducts;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;
using Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;
using Lib.MtgDiscovery.Entry.Queries.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Apis;

/// <summary>
/// Composite entry service for user sealed products collection management.
/// Delegates to specialized command and query entry services.
/// </summary>
internal sealed class UserSealedProductsEntryService : IUserSealedProductsEntryService
{
    private readonly IAddUserSealedProductEntryService _addUserSealedProductEntryService;
    private readonly IUserSealedProductsByUserIdEntryService _userSealedProductsByUserIdEntryService;
    private readonly IUserSealedProductOufToOutMapper _addResultMapper;
    private readonly ICollectionUserSealedProductItrToOutMapper _collectionMapper;

    public UserSealedProductsEntryService(ILogger logger) : this(
        new AddUserSealedProductEntryService(logger),
        new UserSealedProductsByUserIdEntryService(logger),
        new UserSealedProductOufToOutMapper(),
        new CollectionUserSealedProductItrToOutMapper())
    { }

    private UserSealedProductsEntryService(
        IAddUserSealedProductEntryService addUserSealedProductEntryService,
        IUserSealedProductsByUserIdEntryService userSealedProductsByUserIdEntryService,
        IUserSealedProductOufToOutMapper addResultMapper,
        ICollectionUserSealedProductItrToOutMapper collectionMapper)
    {
        _addUserSealedProductEntryService = addUserSealedProductEntryService;
        _userSealedProductsByUserIdEntryService = userSealedProductsByUserIdEntryService;
        _addResultMapper = addResultMapper;
        _collectionMapper = collectionMapper;
    }

    public async Task<IOperationResponse<AddUserSealedProductResultOutEntity>> AddUserSealedProductAsync(
        IAddUserSealedProductArgEntity args)
    {
        IOperationResponse<IUserSealedProductOufEntity> response = await _addUserSealedProductEntryService.Execute(args).ConfigureAwait(false);
        if (response.IsFailure) return new FailureOperationResponse<AddUserSealedProductResultOutEntity>(response.OuterException);

        AddUserSealedProductResultOutEntity outEntity = await _addResultMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<AddUserSealedProductResultOutEntity>(outEntity);
    }

    public async Task<IOperationResponse<List<UserSealedProductOutEntity>>> GetUserSealedProductsByUserIdAsync(
        string userId)
    {
        IOperationResponse<IEnumerable<IUserSealedProductItrEntity>> response = await _userSealedProductsByUserIdEntryService.Execute(userId).ConfigureAwait(false);
        if (response.IsFailure) return new FailureOperationResponse<List<UserSealedProductOutEntity>>(response.OuterException);

        List<UserSealedProductOutEntity> outEntities = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<UserSealedProductOutEntity>>(outEntities);
    }
}
