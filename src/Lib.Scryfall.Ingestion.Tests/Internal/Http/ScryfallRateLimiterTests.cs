using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Internal.Http;

namespace Lib.Scryfall.Ingestion.Tests.Internal.Http;

[TestClass]
public sealed class ScryfallRateLimiterTests
{
    [TestMethod]
    [TestCategory("unit")]
    public async Task AcquireTokenAsync_FirstCall_ReturnsTokenImmediately()
    {
        // Arrange
        using ScryfallRateLimiter rateLimiter = new();

        // Act
        using IRateLimitToken actual = await rateLimiter.AcquireTokenAsync().ConfigureAwait(false);

        // Assert
        _ = actual.Should().NotBeNull();
        _ = actual.AcquiredAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task AcquireTokenAsync_SecondCallWithinDelay_WaitsForDelay()
    {
        // Arrange
        using ScryfallRateLimiter rateLimiter = new();
        using IRateLimitToken firstToken = await rateLimiter.AcquireTokenAsync().ConfigureAwait(false);
        firstToken.Dispose();
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Act
        using IRateLimitToken actual = await rateLimiter.AcquireTokenAsync().ConfigureAwait(false);
        stopwatch.Stop();

        // Assert
        _ = actual.Should().NotBeNull();
        _ = stopwatch.ElapsedMilliseconds.Should().BeGreaterThanOrEqualTo(90); // Allow some tolerance
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task AcquireTokenAsync_SecondCallAfterDelay_ReturnsImmediately()
    {
        // Arrange
        using ScryfallRateLimiter rateLimiter = new();
        using (_ = await rateLimiter.AcquireTokenAsync().ConfigureAwait(false))
        {
            // Simulate waiting for the rate limit to reset
        }
        await Task.Delay(110).ConfigureAwait(false); // Wait more than 100ms
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Act
        using IRateLimitToken actual = await rateLimiter.AcquireTokenAsync().ConfigureAwait(false);
        stopwatch.Stop();

        // Assert
        _ = actual.Should().NotBeNull();
        _ = stopwatch.ElapsedMilliseconds.Should().BeLessThan(50); // Should be immediate
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task AcquireTokenAsync_TokenNotDisposed_BlocksNextAcquire()
    {
        // Arrange
        using ScryfallRateLimiter rateLimiter = new();
        using IRateLimitToken firstToken = await rateLimiter.AcquireTokenAsync().ConfigureAwait(false);

        // Act
        Task<IRateLimitToken> acquireTask = rateLimiter.AcquireTokenAsync();
        await Task.Delay(50).ConfigureAwait(false);

        // Assert
        _ = acquireTask.IsCompleted.Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("unit")]
    public async Task AcquireTokenAsync_TokenDisposed_ReleasesForNextAcquire()
    {
        // Arrange
        using ScryfallRateLimiter rateLimiter = new();
        using IRateLimitToken firstToken = await rateLimiter.AcquireTokenAsync().ConfigureAwait(false);

        // Act
        firstToken.Dispose();
        await Task.Delay(110).ConfigureAwait(false); // Wait for rate limit
        using IRateLimitToken actual = await rateLimiter.AcquireTokenAsync().ConfigureAwait(false);

        // Assert
        _ = actual.Should().NotBeNull();
    }

    [TestMethod]
    [TestCategory("unit")]
    public void Dispose_CalledMultipleTimes_DoesNotThrow()
    {
        // Arrange
        ScryfallRateLimiter rateLimiter = new();
        rateLimiter.Dispose();

        // Act
        Action act = () => rateLimiter.Dispose();

        // Assert
        act.Should().NotThrow();
    }
}
