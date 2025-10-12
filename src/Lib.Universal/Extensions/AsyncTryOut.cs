using System;

namespace Lib.Universal.Extensions;

/// <summary>
/// Represents the result of an async Try operation that would traditionally use an out parameter.
/// Provides a way to return both success/failure status and a value from async methods.
/// </summary>
/// <typeparam name="T">The type of the value being returned. Covariant to support polymorphic usage.</typeparam>
public interface IAsyncTryOut<out T>
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    /// <returns>true if the operation succeeded; otherwise, false.</returns>
    bool IsSuccess();

    /// <summary>
    /// Indicates whether the operation failed.
    /// </summary>
    /// <returns>true if the operation failed; otherwise, false.</returns>
    bool IsFailure() => IsSuccess() is false;

    /// <summary>
    /// Gets the value from the operation.
    /// When IsSuccess() is false, this may return default(T) or a failure response object.
    /// Always check IsSuccess() before using the value.
    /// </summary>
    /// <returns>The value from the operation.</returns>
    T Value();
}

/// <summary>
/// Concrete implementation of IAsyncTryResult that holds both success status and a value.
/// This single implementation handles both success and failure cases through constructor parameters.
/// </summary>
/// <typeparam name="TOut">The type of the value being returned.</typeparam>
public abstract class AsyncTryOut<TOut> : IAsyncTryOut<TOut>
{
    private readonly bool _success;
    private readonly TOut _value;

    /// <summary>
    /// Initializes a new instance of AsyncTryResult.
    /// </summary>
    /// <param name="success">true if the operation succeeded; false if it failed.</param>
    /// <param name="value">The value to return. For success cases, this is the result.
    /// For failure cases, this could be default(TOut) or a failure response object.</param>
    protected AsyncTryOut(bool success, TOut value)
    {
        _success = success;
        _value = value;
    }
    protected AsyncTryOut(bool success)
    {
        _success = success;
        _value = default!;
    }

    /// <inheritdoc/>
    public bool IsSuccess() => _success;

    /// <inheritdoc/>
    public TOut Value() => _value == null ? _value : throw new InvalidOperationException("Cannot access Value() when no value is provided.");
}

public sealed class SuccessAsyncTryOut<TOut> : AsyncTryOut<TOut>
{
    public SuccessAsyncTryOut(TOut value) : base(true, value)
    { }
    public SuccessAsyncTryOut() : base(true)
    { }
}

public sealed class FailureAsyncTryOut<TOut> : AsyncTryOut<TOut>
{
    public FailureAsyncTryOut(TOut value) : base(false, value)
    { }
    public FailureAsyncTryOut() : base(false)
    { }
}
