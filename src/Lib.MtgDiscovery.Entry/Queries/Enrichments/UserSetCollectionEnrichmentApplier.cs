using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal sealed class UserSetCollectionEnrichmentApplier : IUserSetCollectionEnrichmentApplier
{
    private readonly IUserSetCardOufToSetInformationMapper _setInformationMapper;

    public UserSetCollectionEnrichmentApplier() : this(new UserSetCardOufToSetInformationMapper()) { }

    private UserSetCollectionEnrichmentApplier(IUserSetCardOufToSetInformationMapper setInformationMapper) =>
        _setInformationMapper = setInformationMapper;

    public async Task Apply(List<SetItemOutEntity> outEntities, IEnumerable<IUserSetCardOufEntity> userSetCards)
    {
        Dictionary<string, Task<SetInformationOutEntity>> dictionary = userSetCards.ToDictionary(
            usc => usc.SetId,
            usc => _setInformationMapper.Map(usc)
        );

        foreach (SetItemOutEntity set in outEntities)
        {
            if (dictionary.TryGetValue(set.Id, out Task<SetInformationOutEntity> setInformation) is false) continue;
            set.UserCollection = await setInformation.ConfigureAwait(false);
        }
    }
}
