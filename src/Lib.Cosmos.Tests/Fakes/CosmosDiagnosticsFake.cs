using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Tests.Fakes;

internal sealed class CosmosDiagnosticsFake : CosmosDiagnostics
{
    public TimeSpan GetClientElapsedTimeResult { get; init; } = TimeSpan.FromMilliseconds(100);
    public int GetClientElapsedTimeInvokeCount { get; private set; }

    public override TimeSpan GetClientElapsedTime()
    {
        GetClientElapsedTimeInvokeCount++;
        return GetClientElapsedTimeResult;
    }

    public override IReadOnlyList<(string regionName, Uri uri)> GetContactedRegions() => new List<(string, Uri)>();
    public override int GetFailedRequestCount() => 0;
    public override string ToString() => "CosmosDiagnosticsFake";
}
