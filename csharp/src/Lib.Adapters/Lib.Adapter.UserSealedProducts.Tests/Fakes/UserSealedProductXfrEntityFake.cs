using Lib.Adapter.UserSealedProducts.Apis.Entities;

namespace Lib.Adapter.UserSealedProducts.Tests.Fakes;

public sealed class UserSealedProductXfrEntityFake : IUserSealedProductXfrEntity
{
    public string UserId { get; init; }
    public string ProductUuid { get; init; }
    public string SetId { get; init; }
    public int CountDelta { get; init; }
    public string CacheKey => $"user_sealed_product:{UserId}:{ProductUuid}";
}
