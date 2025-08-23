using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Mappers;

public interface ICreateMapper<in TSource, TResult>
{
    Task<TResult> Map(TSource source);
}