using System.Threading.Tasks;
using Lib.BlobStorage.Apis.Operations;
using Lib.BlobStorage.Apis.Operations.Responses;
using Lib.BlobStorage.Tests.Fakes;

namespace Lib.BlobStorage.Tests.Apis.Operations;

[TestClass]
public sealed class BlobInquisitorTests
{
    [TestMethod, TestCategory("unit")]
    public async Task ExistsAsync_ShouldDelegateToSource_WhenCalled()
    {
        // Arrange
        const bool ExpectedResult = true;
        BlobOpResponseFake<bool> responseFake = new()
        { ValueResult = ExpectedResult, HasValueResult = true };
        BlobContainerExistsOperatorFake sourceFake = new()
        {
            ExistsAsyncResult = responseFake
        };

        BlobInquisitor subject = new TestableBlobInquisitor(sourceFake);

        // Act
        BlobOpResponse<bool> actual = await subject.ExistsAsync(null).ConfigureAwait(false);

        // Assert
        actual.Value.Should().Be(ExpectedResult);
        sourceFake.ExistsAsyncInvokeCount.Should().Be(1);
        responseFake.HasValueInvokeCount.Should().Be(0);
    }

    private sealed class TestableBlobInquisitor : BlobInquisitor
    {
        public TestableBlobInquisitor(IBlobContainerExistsOperator source) : base(source) { }
    }
}
