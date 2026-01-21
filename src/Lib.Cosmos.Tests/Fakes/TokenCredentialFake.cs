using System.Threading;
using System.Threading.Tasks;

namespace Lib.Cosmos.Tests.Fakes;

internal sealed class TokenCredentialFake : TokenCredential
{
    public AccessToken GetTokenResult { get; init; }
    public int GetTokenInvokeCount { get; private set; }
    public int GetTokenAsyncInvokeCount { get; private set; }

    public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        GetTokenInvokeCount++;
        return GetTokenResult;
    }

    public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        GetTokenAsyncInvokeCount++;
        return new ValueTask<AccessToken>(GetTokenResult);
    }
}
