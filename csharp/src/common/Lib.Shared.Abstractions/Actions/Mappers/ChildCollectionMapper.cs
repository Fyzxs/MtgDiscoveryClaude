using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Actions.Mappers;

public abstract class ChildCollectionMapper<TChildSource, TChildResult>
{
    private readonly ICreateMapper<TChildSource, TChildResult> _childMapper;

    protected ChildCollectionMapper(ICreateMapper<TChildSource, TChildResult> childMapper)
        => _childMapper = childMapper;

    protected async Task<TChildResult[]> MapChildren(IEnumerable<TChildSource> children)
    {
        return await Task.WhenAll(children.Select(child => _childMapper.Map(child)))
            .ConfigureAwait(false);
    }
}
