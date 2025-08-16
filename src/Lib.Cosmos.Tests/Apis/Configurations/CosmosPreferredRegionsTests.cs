using System.Collections.Generic;
using Lib.Cosmos.Apis.Configurations;
using Lib.Universal.Tests;

namespace Lib.Cosmos.Tests.Apis.Configurations;

[TestClass]
public class CosmosPreferredRegionsTests : BaseToSystemTypeTests<CosmosPreferredRegions, IReadOnlyList<string>>;
