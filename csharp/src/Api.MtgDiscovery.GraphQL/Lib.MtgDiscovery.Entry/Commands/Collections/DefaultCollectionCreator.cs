using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.Collections.Apis;
using Lib.MtgDiscovery.Entry.Commands.Collections.Apis;
using Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.DataModels.Entities.Oufs.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands.Collections;

internal sealed class DefaultCollectionCreator : IDefaultCollectionCreator
{
    private readonly ICollectionsDomainService _domainService;
    private readonly IDefaultCollectionArgToItrMapper _mapper;

    public DefaultCollectionCreator(ILogger logger) : this(
        new CollectionsDomainService(logger),
        new DefaultCollectionArgToItrMapper())
    { }

    private DefaultCollectionCreator(
        ICollectionsDomainService domainService,
        IDefaultCollectionArgToItrMapper mapper)
    {
        _domainService = domainService;
        _mapper = mapper;
    }

    public async Task CreateDefaultCollectionAsync(IUserIdArgEntity args, CancellationToken cancellationToken)
    {
        OwnerIdItrEntity ownerIdItr = new() { OwnerId = args.UserId };
        IOperationResponse<ICollectionOufEntity> existingDefault =
            await _domainService.GetDefaultCollectionAsync(ownerIdItr, cancellationToken).ConfigureAwait(false);

        if (existingDefault.IsSuccess)
        {
            return;
        }

        ICollectionItrEntity itrEntity = await _mapper.Map(args).ConfigureAwait(false);

        await _domainService.CreateCollectionAsync(itrEntity).ConfigureAwait(false);
    }
}
