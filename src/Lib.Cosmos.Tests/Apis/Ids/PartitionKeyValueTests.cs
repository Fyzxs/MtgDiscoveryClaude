using Lib.Cosmos.Apis.Ids;
using Lib.Universal.Tests;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Tests.Apis.Ids;

[TestClass]
public class PartitionKeyValueTests : BaseToSystemTypeTests<PartitionKeyValue, PartitionKey>
{
}
