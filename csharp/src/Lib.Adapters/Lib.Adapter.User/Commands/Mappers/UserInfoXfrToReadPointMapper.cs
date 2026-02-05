using System.Threading.Tasks;
using Lib.Adapter.User.Apis.Entities;
using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.User.Commands.Mappers;

internal sealed class UserInfoXfrToReadPointMapper : IUserInfoXfrToReadPointMapper
{
    private readonly IStringToReadPointItemMapper _stringToReadPointItemMapper;

    public UserInfoXfrToReadPointMapper() : this(new StringToReadPointItemMapper())
    { }

    private UserInfoXfrToReadPointMapper(IStringToReadPointItemMapper stringToReadPointItemMapper) => _stringToReadPointItemMapper = stringToReadPointItemMapper;

    public async Task<ReadPointItem> Map(IUserInfoXfrEntity source) => await _stringToReadPointItemMapper.Map(source.UserId).ConfigureAwait(false);
}
