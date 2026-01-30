using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Collections.Apis;
using Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;
using Lib.MtgDiscovery.Entry.Queries.Collections.Apis;
using Lib.MtgDiscovery.Entry.Queries.Collections.Mappers;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Collections;

internal sealed class CollectionEntryQueryService : ICollectionEntryQueryService
{
    private readonly ICollectionsDomainService _domainService;
    private readonly ICollectionOufListToOutMapper _listMapper;
    private readonly ICollectionOufToOutMapper _oufToOutMapper;

    public CollectionEntryQueryService(ILogger logger) : this(
        new CollectionsDomainService(logger),
        new CollectionOufListToOutMapper(),
        new CollectionOufToOutMapper())
    { }

    private CollectionEntryQueryService(
        ICollectionsDomainService domainService,
        ICollectionOufListToOutMapper listMapper,
        ICollectionOufToOutMapper oufToOutMapper)
    {
        _domainService = domainService;
        _listMapper = listMapper;
        _oufToOutMapper = oufToOutMapper;
    }

    public async Task<IOperationResponse<List<CollectionOutEntity>>> MyCollectionsAsync(IUserIdArgEntity args)
    {
        OwnerIdItrEntity itrEntity = new() { OwnerId = args.UserId };
        IOperationResponse<IEnumerable<ICollectionOufEntity>> opResponse = await _domainService
            .GetCollectionsByOwnerAsync(itrEntity)
            .ConfigureAwait(false);

        if (opResponse.IsFailure)
        {
            return new FailureOperationResponse<List<CollectionOutEntity>>(opResponse.OuterException);
        }

        List<CollectionOutEntity> outEntities = await _listMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<CollectionOutEntity>>(outEntities);
    }

    public async Task<IOperationResponse<CollectionOutEntity>> GetCollectionByIdAsync(ICollectionIdArgEntity args)
    {
        CollectionIdItrEntity itrEntity = new() { CollectionId = args.CollectionId, OwnerId = args.UserId };
        IOperationResponse<ICollectionOufEntity> opResponse = await _domainService
            .GetCollectionByIdAsync(itrEntity)
            .ConfigureAwait(false);

        if (opResponse.IsFailure)
        {
            return new FailureOperationResponse<CollectionOutEntity>(opResponse.OuterException);
        }

        CollectionOutEntity outEntity = await _oufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<CollectionOutEntity>(outEntity);
    }

    public async Task<IOperationResponse<List<CollectionOutEntity>>> SharedCollectionsAsync(IUserIdArgEntity args)
    {
        UserIdItrEntity itrEntity = new() { UserId = args.UserId };
        IOperationResponse<IEnumerable<ICollectionOufEntity>> opResponse = await _domainService
            .GetSharedCollectionsAsync(itrEntity)
            .ConfigureAwait(false);

        if (opResponse.IsFailure)
        {
            return new FailureOperationResponse<List<CollectionOutEntity>>(opResponse.OuterException);
        }

        List<CollectionOutEntity> outEntities = await _listMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<CollectionOutEntity>>(outEntities);
    }

    public async Task<IOperationResponse<List<CollectionOutEntity>>> AccessibleCollectionsAsync(IUserIdArgEntity args)
    {
        UserIdItrEntity itrEntity = new() { UserId = args.UserId };
        IOperationResponse<IEnumerable<ICollectionOufEntity>> opResponse = await _domainService
            .GetAccessibleCollectionsAsync(itrEntity)
            .ConfigureAwait(false);

        if (opResponse.IsFailure)
        {
            return new FailureOperationResponse<List<CollectionOutEntity>>(opResponse.OuterException);
        }

        List<CollectionOutEntity> outEntities = await _listMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<CollectionOutEntity>>(outEntities);
    }
}
