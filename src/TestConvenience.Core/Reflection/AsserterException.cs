using System;

namespace TestConvenience.Core.Reflection;

/// <summary>
/// <see cref="System.Exception" /> thrown when validation is not as expected.
/// </summary>
#pragma warning disable CA1032
public sealed class AsserterException : Exception
#pragma warning restore CA1032
{
    /// <summary>Initializes a new instance of the <see cref="message"></see> class with a specified error message.</summary>
    /// <param name="message">The message that describes the error.</param>
    /// <summary></summary>
    public AsserterException(string message) : base(message) { }
}