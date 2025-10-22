using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cli.MtgDiscovery.DataMigration.OldSystem.AzureSql.Entities;
using Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Entities;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration.Mapping;

internal sealed class OldToNewCardMapper : IOldToNewCardMapper
{
    private readonly ILogger _logger;
    private readonly IOldFinishMapper _finishMapper;
    private readonly IOldSpecialMapper _specialMapper;

    public OldToNewCardMapper(
        ILogger logger,
        IOldFinishMapper finishMapper,
        IOldSpecialMapper specialMapper)
    {
        _logger = logger;
        _finishMapper = finishMapper;
        _specialMapper = specialMapper;
    }

    public async Task<IEnumerable<IAddCardToCollectionArgsEntity>> Map((CollectorDataRecord sqlRecord, OldDiscoveryCardExtEntity oldCosmosCard, ICardItemItrEntity newSystemCard, string targetUserId) source)
    {
        string finish = await _finishMapper
            .Map((source.oldCosmosCard.body.foil, source.oldCosmosCard.body.nonfoil, source.oldCosmosCard.body.etched))
            .ConfigureAwait(false);

        IEnumerable<(string special, int count)> specialEntries = await _specialMapper
            .Map(source.sqlRecord)
            .ConfigureAwait(false);

        List<IAddCardToCollectionArgsEntity> results = specialEntries
            .Select(entry => CreateAddCardEntity(
                source.targetUserId,
                source.newSystemCard.CardId,
                source.newSystemCard.SetId,
                finish,
                entry.special,
                source.newSystemCard.SetGroupId,
                entry.count))
            .ToList();

        return results;
    }

    private IAddCardToCollectionArgsEntity CreateAddCardEntity(
        string userId,
        string cardId,
        string setId,
        string finish,
        string special,
        string setGroupId,
        int count)
    {
        IAuthUserArgEntity authUser = new AuthUserArgEntity(userId);
        IUserCardDetailsArgEntity details = new UserCardDetailsArgEntity(finish, special, setGroupId, count);
        IAddUserCardArgEntity addUserCard = new AddUserCardArgEntity(cardId, setId, userId, details);

        return new AddCardToCollectionArgsEntity(authUser, addUserCard);
    }

    private sealed class AuthUserArgEntity : IAuthUserArgEntity
    {
        public AuthUserArgEntity(string userId)
        {
            UserId = userId;
            SourceId = string.Empty;
            DisplayName = string.Empty;
            Email = string.Empty;
        }

        public string UserId { get; }
        public string SourceId { get; }
        public string DisplayName { get; }
        public string Email { get; }
    }

    private sealed class UserCardDetailsArgEntity : IUserCardDetailsArgEntity
    {
        public UserCardDetailsArgEntity(string finish, string special, string setGroupId, int count)
        {
            Finish = finish;
            Special = special;
            SetGroupId = setGroupId;
            Count = count;
        }

        public string Finish { get; }
        public string Special { get; }
        public string SetGroupId { get; }
        public int Count { get; }
    }

    private sealed class AddUserCardArgEntity : IAddUserCardArgEntity
    {
        public AddUserCardArgEntity(string cardId, string setId, string userId, IUserCardDetailsArgEntity details)
        {
            CardId = cardId;
            SetId = setId;
            UserId = userId;
            UserCardDetails = details;
        }

        public string CardId { get; }
        public string SetId { get; }
        public string UserId { get; }
        public IUserCardDetailsArgEntity UserCardDetails { get; }
    }

    private sealed class AddCardToCollectionArgsEntity : IAddCardToCollectionArgsEntity
    {
        public AddCardToCollectionArgsEntity(IAuthUserArgEntity authUser, IAddUserCardArgEntity addUserCard)
        {
            AuthUser = authUser;
            AddUserCard = addUserCard;
        }

        public IAuthUserArgEntity AuthUser { get; }
        public IAddUserCardArgEntity AddUserCard { get; }
    }
}
