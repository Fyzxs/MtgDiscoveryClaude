using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Commands.Entities;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

internal sealed class AddUserCardRollbackXfrMapper : IAddUserCardRollbackXfrMapper
{
    public Task<IAddUserCardXfrEntity> Map(IAddUserCardXfrEntity source)
    {
        UserCardDetailsXfrEntity negatedDetails = new()
        {
            Finish = source.Details.Finish,
            Special = source.Details.Special,
            Count = -source.Details.Count,
            SetGroupId = source.Details.SetGroupId
        };

        IAddUserCardXfrEntity rollback = new AddUserCardXfrEntity
        {
            UserId = source.UserId,
            CardId = source.CardId,
            SetId = source.SetId,
            CardName = source.CardName,
            SetName = source.SetName,
            SetCode = source.SetCode,
            ReleasedAt = source.ReleasedAt,
            Artist = source.Artist,
            ArtistIds = source.ArtistIds,
            CardNameGuid = source.CardNameGuid,
            Details = negatedDetails
        };
        return Task.FromResult(rollback);
    }
}
