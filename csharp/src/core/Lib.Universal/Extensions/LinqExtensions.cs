using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lib.Universal.Extensions;

/// <summary>
/// Provides extension methods for LINQ operations with asynchronous support.
/// </summary>
public static class LinqExtensions
{
    /// <summary>
    /// Determines whether all elements of a sequence satisfy a condition asynchronously.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The sequence of elements to test.</param>
    /// <param name="predicate">An asynchronous function to test each element for a condition.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains <c>true</c> if all elements satisfy the condition;
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="source"/> or <paramref name="predicate"/> is <c>null</c>.
    /// </exception>
    public static async Task<bool> AllAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, Task<bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        foreach (TSource item in source)
        {
            if (item is null) return false;

            bool result = await predicate(item).ConfigureAwait(false);
            if (result is false) return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether all elements of an asynchronous sequence satisfy a condition synchronously.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The sequence of asynchronous tasks producing elements to test.</param>
    /// <param name="predicate">A synchronous function to test each element for a condition.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains <c>true</c> if all elements satisfy the condition;
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="source"/> or <paramref name="predicate"/> is <c>null</c>.
    /// </exception>
    public static async Task<bool> AllAsync<TSource>(this IEnumerable<Task<TSource>> source, Func<TSource, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        foreach (Task<TSource> item in source)
        {
            TSource awaitedItem = await item.ConfigureAwait(false);
            if (predicate(awaitedItem) is false) return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether none of the elements of a sequence satisfy a condition asynchronously.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The sequence of asynchronous tasks producing elements to test.</param>
    /// <param name="predicate">A synchronous function to test each element for a condition.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains <c>true</c> if no elements satisfy the condition;
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="source"/> or <paramref name="predicate"/> is <c>null</c>.
    /// </exception>
    public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        return source.Any(predicate) is false;
    }
}
