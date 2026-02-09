using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Actions.Mappers;

public abstract class CollectionCreateMapper<TSource, TResult> : ICreateMapper<IEnumerable<TSource>, IEnumerable<TResult>>
{
    private readonly ICreateMapper<TSource, TResult> _mapper;

    protected CollectionCreateMapper(ICreateMapper<TSource, TResult> mapper) => _mapper = mapper;

    public async Task<IEnumerable<TResult>> Map(IEnumerable<TSource> source)
    {
        ICollection<Task<TResult>> tasks = [.. source.Select(item => _mapper.Map(item))];
        TResult[] results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return results;
    }
}
