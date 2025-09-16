using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Mappers;

/// <summary>
/// Defines a mapper that creates a new instance of <typeparamref name="TResult"/> from a single source object.
/// </summary>
/// <typeparam name="TSource">The type of the source object to map from.</typeparam>
/// <typeparam name="TResult">The type of the result object to create.</typeparam>
public interface ICreateMapper<in TSource, TResult>
{
    /// <summary>
    /// Maps the source object to a new instance of the result type.
    /// </summary>
    /// <param name="source">The source object to map from.</param>
    /// <returns>A task that represents the asynchronous mapping operation, containing the created result object.</returns>
    Task<TResult> Map(TSource source);
}

/// <summary>
/// Defines a mapper that creates a new instance of <typeparamref name="TResult"/> from two source objects.
/// </summary>
/// <typeparam name="TSourceFirst">The type of the first source object to map from.</typeparam>
/// <typeparam name="TSourceSecond">The type of the second source object to map from.</typeparam>
/// <typeparam name="TResult">The type of the result object to create.</typeparam>
public interface ICreateMapper<in TSourceFirst, in TSourceSecond, TResult>
{
    /// <summary>
    /// Maps two source objects to a new instance of the result type.
    /// </summary>
    /// <param name="source1">The first source object to map from.</param>
    /// <param name="source2">The second source object to map from.</param>
    /// <returns>A task that represents the asynchronous mapping operation, containing the created result object.</returns>
    Task<TResult> Map(TSourceFirst source1, TSourceSecond source2);
}
