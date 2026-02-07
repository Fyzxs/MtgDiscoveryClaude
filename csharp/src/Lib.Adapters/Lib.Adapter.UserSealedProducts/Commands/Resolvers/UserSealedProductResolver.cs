using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.UserSealedProducts.Commands.Resolvers;

internal sealed class UserSealedProductResolver : IUserSealedProductResolver
{
    public UserSealedProductExtEntity Resolve(OpResponse<UserSealedProductExtEntity> input, IUserSealedProductXfrEntity context)
    {
        if (input.IsSuccessful())
            return input.Value;

        return new UserSealedProductExtEntity
        {
            UserId = context.UserId,
            ProductUuid = context.ProductUuid,
            SetId = context.SetId,
            Count = 0
        };
    }
}
