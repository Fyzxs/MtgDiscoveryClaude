using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers.Collections;

internal sealed class TransferCollectionOwnershipArgsMapper : ITransferCollectionOwnershipArgsMapper
{
    public Task<ITransferCollectionOwnershipArgsEntity> Map(ClaimsPrincipal claimsPrincipal, ITransferCollectionOwnershipArgEntity transferOwnership)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);

        ITransferCollectionOwnershipArgsEntity entity = new TransferCollectionOwnershipArgsEntity
        {
            AuthUser = authUser,
            TransferOwnership = transferOwnership
        };

        return Task.FromResult(entity);
    }
}
