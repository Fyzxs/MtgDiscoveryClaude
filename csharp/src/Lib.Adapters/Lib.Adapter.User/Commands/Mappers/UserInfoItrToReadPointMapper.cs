using System.Threading.Tasks;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities.Itrs.User;

namespace Lib.Adapter.User.Commands.Mappers;

internal sealed class UserInfoItrToReadPointMapper : IUserInfoItrToReadPointMapper
{
    private readonly IStringToReadPointItemMapper _stringToReadPointItemMapper;

    public UserInfoItrToReadPointMapper() : this(new StringToReadPointItemMapper())
    { }

    private UserInfoItrToReadPointMapper(IStringToReadPointItemMapper stringToReadPointItemMapper) => _stringToReadPointItemMapper = stringToReadPointItemMapper;

    public async Task<ReadPointItem> Map(IUserInfoItrEntity source) => await _stringToReadPointItemMapper.Map(source.UserId).ConfigureAwait(false);
}
