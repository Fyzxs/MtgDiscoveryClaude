using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Mappers;

public interface IMapper<in TSource, in TResult>
{
    Task Map(TSource source, TResult result);
}
