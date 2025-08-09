using System;
using System.Net;
using Lib.Universal.Primitives;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Apis.Operators;

/// <summary>
/// Represents the response from a Cosmos DB operation.
/// </summary>
/// <typeparam name="T">The type of the response value.</typeparam>
public abstract class OpResponse<T> : ToSystemType<T>
{
    /// <summary>
    /// Gets the value from the operation response.
    /// </summary>
    public abstract T Value { get; }

    /// <summary>
    /// Gets the exception
    /// </summary>
    public virtual CosmosException Exception() => throw new InvalidOperationException("No exception provided");

    /// <summary>
    /// Gets the HTTP status code from the operation response.
    /// </summary>
    public abstract HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Checks if the operation was successful based on the status code.
    /// </summary>
    /// <returns>True if the status code indicates success; otherwise, false.</returns>
    public bool IsSuccessful() => 200 <= (int)StatusCode && (int)StatusCode <= 299;

    /// <summary>
    /// Checks if the operation was not successful based on the status code.
    /// </summary>
    /// <returns>True if the status code indicates failure; otherwise, false.</returns>
    public bool IsNotSuccessful() => !IsSuccessful();

    /// <summary>
    /// Returns the value from the operation response.
    /// </summary>
    /// <returns>The operation response value.</returns>
    public override T AsSystemType() => Value;
}
