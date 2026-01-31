using Lib.Cosmos.Apis.Configurations;
using Lib.Universal.Tests;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Tests.Apis.Configurations;

[TestClass]
public sealed class CosmosConnectionModeTests : BaseToSystemTypeTests<CosmosConnectionMode, ConnectionMode>;
