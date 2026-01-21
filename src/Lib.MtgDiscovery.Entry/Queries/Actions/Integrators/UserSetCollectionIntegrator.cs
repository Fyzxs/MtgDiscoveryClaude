using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Integrators;

internal sealed class UserSetCollectionIntegrator : IUserSetCollectionIntegrator
{
    private readonly IUserSetCardOufToSetInformationMapper _setInformationMapper;

    public UserSetCollectionIntegrator() : this(new UserSetCardOufToSetInformationMapper()) { }

    private UserSetCollectionIntegrator(IUserSetCardOufToSetInformationMapper setInformationMapper) =>
        _setInformationMapper = setInformationMapper;

    public async Task<List<SetItemOutEntity>> Integrate(List<SetItemOutEntity> current, IEnumerable<IUserSetCardOufEntity> change)
    {
        Dictionary<string, Task<SetInformationOutEntity>> dictionary = change.ToDictionary(
            usc => usc.SetId,
            usc => _setInformationMapper.Map(usc)
        );

        foreach (SetItemOutEntity set in current)
        {
            if (dictionary.TryGetValue(set.Id, out Task<SetInformationOutEntity> setInformation) is false) continue;
            set.UserCollection = await setInformation.ConfigureAwait(false);
        }

        return current;
    }
}
