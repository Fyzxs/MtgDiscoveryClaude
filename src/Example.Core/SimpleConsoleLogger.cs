using System;

namespace Example.Core;

public sealed class SimpleConsoleLogger : ISimpleLogger
{
    public void Log(string message) => Console.WriteLine($"{DateTime.UtcNow:s} | {message}");
}
