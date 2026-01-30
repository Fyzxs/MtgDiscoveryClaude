using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.Collections;

namespace Lib.Domain.Collections.Apis;

public interface ICollectionAuthorizationService
{
    bool IsOwner(string ownerId, string userId);
    bool IsAdmin(IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers, string userId);
    bool IsOwnerOrAdmin(string ownerId, IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers, string userId);
    bool IsEditor(IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers, string userId);
    bool IsViewer(IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers, string userId);
    bool IsAuthorizedUser(IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers, string userId);
    bool CanGrantAccess(string ownerId, IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers, string userId);
    bool CanRevokeAccess(string ownerId, IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers, string revokerId, string targetUserId);
}
