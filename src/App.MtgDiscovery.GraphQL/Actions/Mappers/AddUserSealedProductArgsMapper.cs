using System;
using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Authentication;
using App.MtgDiscovery.GraphQL.Entities.Args.UserSealedProducts;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers;

internal sealed class AddUserSealedProductArgsMapper : IAddUserSealedProductArgsMapper
{
    public Task<IAddUserSealedProductArgsEntity> Map(ClaimsPrincipal claimsPrincipal, IAddUserSealedProductClientArgEntity addUserSealedProduct)
    {
        AuthUserArgEntity authUser = new(claimsPrincipal);

        // Validate that the authenticated user has permission to modify this collection
        // For now, we validate that collectionId matches userId (1:1 mapping)
        // Future enhancement: support shared collections with permission checks
        if (string.IsNullOrEmpty(addUserSealedProduct.CollectionId))
        {
            throw new ArgumentException("CollectionId is required", nameof(addUserSealedProduct));
        }

        if (authUser.UserId.Equals(addUserSealedProduct.CollectionId, StringComparison.Ordinal) is false)
        {
            throw new UnauthorizedAccessException($"User {authUser.UserId} is not authorized to modify collection {addUserSealedProduct.CollectionId}");
        }

        IAddUserSealedProductArgEntity serverEntity = new AddUserSealedProductServerArgEntity
        {
            UserId = authUser.UserId,
            CollectionId = addUserSealedProduct.CollectionId,
            ProductUuid = addUserSealedProduct.ProductUuid,
            SetId = addUserSealedProduct.SetId,
            CountDelta = addUserSealedProduct.CountDelta
        };

        IAddUserSealedProductArgsEntity entity = new AddUserSealedProductArgsEntity
        {
            AuthUser = authUser,
            AddUserSealedProduct = serverEntity
        };

        return Task.FromResult(entity);
    }
}
