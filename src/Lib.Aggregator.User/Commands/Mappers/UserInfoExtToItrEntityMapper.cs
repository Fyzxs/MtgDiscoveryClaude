using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.User.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.User.Commands.Mappers;

/// <summary>
/// Maps UserInfoExtEntity to IUserInfoItrEntity.
/// </summary>
internal sealed class UserInfoExtToItrEntityMapper : IUserInfoExtToItrEntityMapper
{

    public Task<IUserInfoItrEntity> Map([NotNull] UserInfoExtEntity source)
    {

        return Task.FromResult<IUserInfoItrEntity>(new UserInfoItrEntity
        {
            UserId = source.UserId,
            UserNickname = source.DisplayName,
            UserSourceId = source.SourceId
        });
    }
}
