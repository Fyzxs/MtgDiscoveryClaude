using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.UserSealedProducts;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;
using Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;
using Lib.MtgDiscovery.Entry.Queries.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Apis;

internal sealed class UserSealedProductsEntryService : IUserSealedProductsEntryService
{
    private readonly IAddUserSealedProductEntryService _addUserSealedProductEntryService;
    private readonly IUserSealedProductsByUserIdEntryService _userSealedProductsByUserIdEntryService;
    private readonly ICollectionUserSealedProductOufToOutMapper _collectionMapper;

    public UserSealedProductsEntryService(ILogger logger) : this(
        new AddUserSealedProductEntryService(logger),
        new UserSealedProductsByUserIdEntryService(logger),
        new CollectionUserSealedProductOufToOutMapper())
    { }

    private UserSealedProductsEntryService(
        IAddUserSealedProductEntryService addUserSealedProductEntryService,
        IUserSealedProductsByUserIdEntryService userSealedProductsByUserIdEntryService,
        ICollectionUserSealedProductOufToOutMapper collectionMapper)
    {
        _addUserSealedProductEntryService = addUserSealedProductEntryService;
        _userSealedProductsByUserIdEntryService = userSealedProductsByUserIdEntryService;
        _collectionMapper = collectionMapper;
    }

    public async Task<IOperationResponse<List<SealedProductOutEntity>>> AddSealedProductToCollectionAsync(
        IAddSealedProductToCollectionArgsEntity args, CancellationToken cancellationToken) => await _addUserSealedProductEntryService.Execute(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<UserSealedProductOutEntity>>> GetUserSealedProductsByUserIdAsync(
        string userId, CancellationToken cancellationToken)
    {
        IOperationResponse<IEnumerable<IUserSealedProductOufEntity>> response = await _userSealedProductsByUserIdEntryService.Execute(userId, cancellationToken).ConfigureAwait(false);
        if (response.IsFailure)
        { return new FailureOperationResponse<List<UserSealedProductOutEntity>>(response.OuterException); }

        List<UserSealedProductOutEntity> outEntities = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<UserSealedProductOutEntity>>(outEntities);
    }
}
