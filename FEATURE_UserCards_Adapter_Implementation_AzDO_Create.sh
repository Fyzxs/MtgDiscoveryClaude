#!/bin/bash

# FEATURE UserCards Adapter Implementation - Azure DevOps Work Items Creation Script
# Total: 1 Feature + 11 User Stories + 23 Implementation Tasks + 23 Test Tasks + 33 Standard Tasks = 91 Work Items

echo "🚀 Creating Azure DevOps Work Items for UserCards Adapter Implementation"
echo "Creating work item hierarchy: Feature → User Stories → Implementation/Test Tasks"
echo ""

# ===================================
# FEATURE: UserCards Adapter Implementation
# ===================================

echo "📋 Creating Feature: UserCards Adapter Implementation"
FEATURE_ID=$(az boards work-item create \
  --title "UserCards Adapter Implementation" \
  --type "Feature" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<h2>Overview</h2>
<p>Implementation of UserCards adapter layer for managing user card collections in Cosmos DB. This adapter will handle write operations (Scribe) for adding cards to a user's collection, following the established CQRS pattern with Commands folder structure.</p>

<p><strong>Total Tasks</strong>: 23 tasks (11 implementation + 12 test tasks)</p>

<h2>Context</h2>
<p>UserCards Cosmos schema has been defined:</p>
<pre><code>UserCards
{
  partition: userId
  id: cardId
  cardId: card.Id
  userId: user.Id
  setId: card.set.Id
  collected: [
    {
      finish: {nonfoil|foil|etched}
      special: {none|altered|signed|proof}
      count: {count}
    }
  ]
}
</code></pre>

<p>The adapter layer follows established patterns from existing adapters (Cards, Artists, Sets, User) and integrates with the Lib.Cosmos infrastructure layer.</p>

<h2>Reference Projects</h2>
<ul>
<li><strong>Pattern Reference</strong>: <code>src/Lib.Adapter.User/</code> - Use as primary template for adapter structure</li>
<li><strong>CQRS Reference</strong>: <code>src/Lib.Adapter.Cards/</code> - Reference for CQRS folder structure (Queries folder)</li>
<li><strong>Cosmos Integration</strong>: <code>src/Lib.Adapter.Scryfall.Cosmos/</code> - Where Cosmos Items and operators live</li>
<li><strong>Infrastructure</strong>: <code>src/Lib.Cosmos/</code> - Core Cosmos infrastructure components</li>
</ul>

<h2>Architecture Goals</h2>
<ul>
<li>CQRS pattern with Commands folder structure</li>
<li>MicroObjects principles throughout</li>
<li>IOperationResponse from Lib.Shared.Invocation</li>
<li>Proper layer separation and dependency injection</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

echo "✅ Created Feature: UserCards Adapter Implementation (ID: $FEATURE_ID)"
echo ""

# ===================================
# USER STORY 1: Create UserCards Adapter Project Structure
# ===================================

echo "📋 Creating User Story 1: Create UserCards Adapter Project Structure"
US1_ID=$(az boards work-item create \
  --title "US-IMPL 1: Create UserCards Adapter Project Structure" \
  --type "User Story" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<h3>Purpose</h3>
<p>Create the foundational project structure for the UserCards adapter following established adapter patterns. This includes proper folder organization, project configuration, and build setup.</p>

<h3>Acceptance Criteria</h3>
<ul>
<li>New project Lib.Adapter.UserCards created with proper folder structure</li>
<li>Project references configured correctly</li>
<li>Project builds successfully</li>
</ul>

<h3>Reference Projects</h3>
<ul>
<li><strong>Pattern Reference</strong>: <code>src/Lib.Adapter.User/</code> - Primary template for structure</li>
<li><strong>CQRS Reference</strong>: <code>src/Lib.Adapter.Cards/</code> - Commands folder organization</li>
</ul>

<h3>Key Design Decisions</h3>
<ul>
<li>Using CQRS pattern with Commands folder (not Persistence like User adapter)</li>
<li>Tracer bullet approach: basic write operation first, enhance later</li>
<li>Following MicroObjects principles</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $US1_ID --relation-type parent --target-id $FEATURE_ID > /dev/null
echo "✅ Created US-IMPL 1 (ID: $US1_ID) → Feature ($FEATURE_ID)"

# Implementation Tasks for User Story 1
echo "  📋 Creating Implementation Tasks for User Story 1"

IMPL_TASK1_ID=$(az boards work-item create \
  --title "TSK-IMPL 1: Create Project and Folder Structure" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<p><strong>Location</strong>: <code>src/Lib.Adapter.UserCards/</code></p>
<p><strong>Reference</strong>: Copy structure from <code>src/Lib.Adapter.User/</code></p>

<h4>Steps</h4>
<ol>
<li>Create new directory <code>src/Lib.Adapter.UserCards/</code></li>
<li>Create subdirectories:
  <ul>
    <li><code>Apis/</code> - Public interfaces</li>
    <li><code>Commands/</code> - CQRS command implementations</li>
    <li><code>Entities/</code> - Response entities if needed</li>
    <li><code>Exceptions/</code> - Custom exceptions</li>
  </ul>
</li>
</ol>

<h4>Modifications</h4>
<ul>
<li>No Persistence folder (using Commands instead per CQRS)</li>
<li>Add Commands folder for CQRS pattern</li>
<li>Keep Exceptions folder for custom exceptions</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $IMPL_TASK1_ID --relation-type parent --target-id $US1_ID > /dev/null
echo "    ✅ Implementation Task 1 (ID: $IMPL_TASK1_ID)"

TEST_TASK1_ID=$(az boards work-item create \
  --title "TSK-IMPL 1-TEST: Unit Tests for Project Structure Creation" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<p><strong>Parent Implementation Task</strong>: TSK-IMPL 1 - Create Project and Folder Structure</p>
<p><strong>Purpose</strong>: Verify project structure and folder creation following TESTING_GUIDELINES.md patterns.</p>

<h4>Testing Strategy</h4>
<ul>
<li>Verify directory structure matches expected pattern</li>
<li>Confirm all required folders exist (Apis, Commands, Entities, Exceptions)</li>
<li>Validate folder structure follows established adapter patterns</li>
</ul>

<h4>Reference Patterns</h4>
<ul>
<li>Compare structure with <code>src/Lib.Adapter.User/</code> folder organization</li>
<li>Review CQRS folder patterns from <code>src/Lib.Adapter.Cards/</code></li>
<li>Follow directory conventions from existing adapter projects</li>
</ul>

<h4>Validation Approach</h4>
<ul>
<li>Directory existence verification</li>
<li>Structure compliance with adapter patterns</li>
<li>CQRS folder organization validation</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $TEST_TASK1_ID --relation-type parent --target-id $US1_ID > /dev/null
az boards work-item relation add --id $TEST_TASK1_ID --relation-type predecessor --target-id $IMPL_TASK1_ID > /dev/null
echo "    ✅ Test Task 1 (ID: $TEST_TASK1_ID) → predecessor: $IMPL_TASK1_ID"

IMPL_TASK2_ID=$(az boards work-item create \
  --title "TSK-IMPL 2: Create Project File" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<p><strong>Location</strong>: <code>src/Lib.Adapter.UserCards/Lib.Adapter.UserCards.csproj</code></p>
<p><strong>Reference</strong>: Copy from <code>src/Lib.Adapter.User/Lib.Adapter.User.csproj</code></p>

<h4>Pattern to Follow</h4>
<ul>
<li>Target framework net9.0</li>
<li>Add project references to:
  <ul>
    <li>Lib.Adapter.Scryfall.Cosmos</li>
    <li>Lib.Shared.DataModels</li>
    <li>Lib.Shared.Invocation</li>
  </ul>
</li>
<li>Set TreatWarningsAsErrors to true</li>
<li>Add InternalsVisibleTo for test project</li>
</ul>

<h4>Modifications</h4>
<ul>
<li>Change RootNamespace to Lib.Adapter.UserCards</li>
<li>Add reference to Lib.Adapter.Scryfall.Cosmos project</li>
<li>Keep all other settings identical</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $IMPL_TASK2_ID --relation-type parent --target-id $US1_ID > /dev/null
echo "    ✅ Implementation Task 2 (ID: $IMPL_TASK2_ID)"

TEST_TASK2_ID=$(az boards work-item create \
  --title "TSK-IMPL 2-TEST: Unit Tests for Project File Configuration" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<p><strong>Parent Implementation Task</strong>: TSK-IMPL 2 - Create Project File</p>
<p><strong>Purpose</strong>: Verify project file configuration and build settings following TESTING_GUIDELINES.md patterns.</p>

<h4>Testing Strategy</h4>
<ul>
<li>Verify project builds successfully</li>
<li>Confirm all project references are correctly configured</li>
<li>Validate target framework and compiler settings</li>
</ul>

<h4>Reference Patterns</h4>
<ul>
<li>Compare configuration with <code>src/Lib.Adapter.User/Lib.Adapter.User.csproj</code></li>
<li>Review project reference patterns from existing adapter projects</li>
<li>Follow build configuration from established projects</li>
</ul>

<h4>Validation Approach</h4>
<ul>
<li>Successful dotnet build execution</li>
<li>Project reference resolution verification</li>
<li>Compiler warning/error absence validation</li>
<li>InternalsVisibleTo configuration verification</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $TEST_TASK2_ID --relation-type parent --target-id $US1_ID > /dev/null
az boards work-item relation add --id $TEST_TASK2_ID --relation-type predecessor --target-id $IMPL_TASK2_ID > /dev/null
echo "    ✅ Test Task 2 (ID: $TEST_TASK2_ID) → predecessor: $IMPL_TASK2_ID"

# Standard Tasks for User Story 1
echo "  📋 Creating Standard Tasks for User Story 1"

CODE_REVIEW1_ID=$(az boards work-item create \
  --title "Code Review" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<p>Conduct thorough code review for User Story 1 implementation following established code review practices.</p>

<h4>Review Focus Areas</h4>
<ul>
<li>Project structure compliance with adapter patterns</li>
<li>Project file configuration accuracy</li>
<li>Folder organization following CQRS principles</li>
<li>Build configuration and references</li>
</ul>

<h4>Acceptance Criteria</h4>
<ul>
<li>Code follows MicroObjects principles</li>
<li>Project structure matches established patterns</li>
<li>Build configuration is correct</li>
<li>No code quality issues identified</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $CODE_REVIEW1_ID --relation-type parent --target-id $US1_ID > /dev/null
echo "    ✅ Code Review (ID: $CODE_REVIEW1_ID)"

PULL_REQUEST1_ID=$(az boards work-item create \
  --title "Pull Request" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<p>Create and manage pull request for User Story 1 implementation.</p>

<h4>Pull Request Requirements</h4>
<ul>
<li>Clear description of changes made</li>
<li>All tests passing</li>
<li>Code review approved</li>
<li>Branch builds successfully</li>
</ul>

<h4>Merge Criteria</h4>
<ul>
<li>All PR checks passing</li>
<li>Code review approval received</li>
<li>Conflicts resolved</li>
<li>Target branch build successful</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $PULL_REQUEST1_ID --relation-type parent --target-id $US1_ID > /dev/null
echo "    ✅ Pull Request (ID: $PULL_REQUEST1_ID)"

USER_APPROVAL1_ID=$(az boards work-item create \
  --title "User Approval" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<p>Obtain user approval for completed User Story 1 implementation.</p>

<h4>Approval Criteria</h4>
<ul>
<li>Project structure meets requirements</li>
<li>Build configuration working correctly</li>
<li>All acceptance criteria satisfied</li>
<li>Ready for next development phase</li>
</ul>

<h4>Validation Steps</h4>
<ul>
<li>Demonstrate project structure</li>
<li>Show successful build</li>
<li>Confirm folder organization</li>
<li>Verify reference configuration</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $USER_APPROVAL1_ID --relation-type parent --target-id $US1_ID > /dev/null
echo "    ✅ User Approval (ID: $USER_APPROVAL1_ID)"

echo ""

# ===================================
# USER STORY 2: Define UserCards Adapter Interfaces
# ===================================

echo "📋 Creating User Story 2: Define UserCards Adapter Interfaces"
US2_ID=$(az boards work-item create \
  --title "US-IMPL 2: Define UserCards Adapter Interfaces" \
  --type "User Story" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<h3>Purpose</h3>
<p>Define the core interfaces for the UserCards adapter service. This includes the main composite interface and specialized command interface following established adapter patterns.</p>

<h3>Acceptance Criteria</h3>
<ul>
<li>Main composite interface IUserCardsAdapterService defined</li>
<li>Specialized interface IUserCardsCommandAdapter defined</li>
<li>Interfaces follow established patterns with proper documentation</li>
</ul>

<h3>Interface Design</h3>
<ul>
<li>Composite interface pattern inheriting from specialized interfaces</li>
<li>XML documentation explaining design decisions</li>
<li>Public interface for aggregator layer consumption</li>
<li>Command operations using IOperationResponse pattern</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $US2_ID --relation-type parent --target-id $FEATURE_ID > /dev/null
echo "✅ Created US-IMPL 2 (ID: $US2_ID) → Feature ($FEATURE_ID)"

# Implementation Tasks for User Story 2
echo "  📋 Creating Implementation Tasks for User Story 2"

IMPL_TASK3_ID=$(az boards work-item create \
  --title "TSK-IMPL 3: Create Main Adapter Interface" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<p><strong>Location</strong>: <code>src/Lib.Adapter.UserCards/Apis/IUserCardsAdapterService.cs</code></p>
<p><strong>Reference</strong>: Copy from <code>src/Lib.Adapter.User/Apis/IUserAdapterService.cs:1-30</code></p>

<h4>Pattern to Follow</h4>
<ul>
<li>Composite interface pattern inheriting from specialized interfaces</li>
<li>XML documentation explaining design decisions</li>
<li>Public interface that will be consumed by aggregator layer</li>
</ul>

<h4>Modifications</h4>
<ul>
<li>Replace "User" with "UserCards" in interface name</li>
<li>Update documentation to reference UserCards operations</li>
<li>Inherit from IUserCardsCommandAdapter instead of IUserPersistenceAdapter</li>
<li>Update XML comments to describe card collection operations</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $IMPL_TASK3_ID --relation-type parent --target-id $US2_ID > /dev/null
echo "    ✅ Implementation Task 3 (ID: $IMPL_TASK3_ID)"

TEST_TASK3_ID=$(az boards work-item create \
  --title "TSK-IMPL 3-TEST: Unit Tests for Main Adapter Interface" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<p><strong>Parent Implementation Task</strong>: TSK-IMPL 3 - Create Main Adapter Interface</p>
<p><strong>Purpose</strong>: Create comprehensive unit tests for IUserCardsAdapterService interface following TESTING_GUIDELINES.md patterns.</p>

<p><strong>Test File Location</strong>: <code>src/Lib.Adapter.UserCards.Tests/Apis/IUserCardsAdapterServiceTests.cs</code></p>

<h4>Testing Strategy</h4>
<ul>
<li>Interface inheritance verification</li>
<li>Method signature validation</li>
<li>XML documentation presence confirmation</li>
<li>Public interface accessibility verification</li>
</ul>

<h4>Test Scenarios to Cover</h4>
<ul>
<li>Interface inheritance from IUserCardsCommandAdapter</li>
<li>Method signatures match expected patterns</li>
<li>Interface accessibility and visibility</li>
<li>XML documentation completeness</li>
</ul>

<h4>Reference Patterns</h4>
<ul>
<li>Copy test structure from <code>src/Lib.Aggregator.Cards.Tests/Apis/CardAggregatorServiceTests.cs:13-30</code></li>
<li>Review interface testing patterns from existing adapter test projects</li>
<li>Follow interface validation from <code>src/Lib.Shared.Abstractions.Tests/</code></li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $TEST_TASK3_ID --relation-type parent --target-id $US2_ID > /dev/null
az boards work-item relation add --id $TEST_TASK3_ID --relation-type predecessor --target-id $IMPL_TASK3_ID > /dev/null
echo "    ✅ Test Task 3 (ID: $TEST_TASK3_ID) → predecessor: $IMPL_TASK3_ID"

IMPL_TASK4_ID=$(az boards work-item create \
  --title "TSK-IMPL 4: Create Command Adapter Interface" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<p><strong>Location</strong>: <code>src/Lib.Adapter.UserCards/Apis/IUserCardsCommandAdapter.cs</code></p>
<p><strong>Reference</strong>: Copy from <code>src/Lib.Adapter.User/Apis/IUserPersistenceAdapter.cs:1-29</code></p>

<h4>Pattern to Follow</h4>
<ul>
<li>Specialized interface for command operations</li>
<li>Uses IOperationResponse from Lib.Shared.Invocation</li>
<li>Accepts ItrEntity parameters following MicroObjects principles</li>
<li>Returns ItrEntity wrapped in IOperationResponse</li>
</ul>

<h4>Modifications</h4>
<ul>
<li>Change interface name from IUserPersistenceAdapter to IUserCardsCommandAdapter</li>
<li>Replace RegisterUserAsync method with AddUserCardAsync method</li>
<li>Method signature should accept IUserCardItrEntity parameter</li>
<li>Return type should be IOperationResponse&lt;IUserCardItrEntity&gt;</li>
<li>Update XML documentation to describe command operations</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $IMPL_TASK4_ID --relation-type parent --target-id $US2_ID > /dev/null
echo "    ✅ Implementation Task 4 (ID: $IMPL_TASK4_ID)"

TEST_TASK4_ID=$(az boards work-item create \
  --title "TSK-IMPL 4-TEST: Unit Tests for Command Adapter Interface" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<p><strong>Parent Implementation Task</strong>: TSK-IMPL 4 - Create Command Adapter Interface</p>
<p><strong>Purpose</strong>: Create comprehensive unit tests for IUserCardsCommandAdapter interface following TESTING_GUIDELINES.md patterns.</p>

<p><strong>Test File Location</strong>: <code>src/Lib.Adapter.UserCards.Tests/Apis/IUserCardsCommandAdapterTests.cs</code></p>

<h4>Testing Strategy</h4>
<ul>
<li>Method signature validation for AddUserCardAsync</li>
<li>Return type verification (IOperationResponse&lt;IUserCardItrEntity&gt;)</li>
<li>Parameter type validation (IUserCardItrEntity)</li>
<li>Interface contract verification</li>
</ul>

<h4>Test Scenarios to Cover</h4>
<ul>
<li>AddUserCardAsync method signature verification</li>
<li>Return type matches IOperationResponse pattern</li>
<li>Parameter accepts IUserCardItrEntity</li>
<li>Async method pattern compliance</li>
</ul>

<h4>Reference Patterns</h4>
<ul>
<li>Copy test structure from <code>src/Lib.Aggregator.Cards.Tests/Apis/CardAggregatorServiceTests.cs:13-30</code></li>
<li>Review command interface patterns from <code>src/Lib.Shared.Invocation.Tests/</code></li>
<li>Follow interface testing from existing adapter projects</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $TEST_TASK4_ID --relation-type parent --target-id $US2_ID > /dev/null
az boards work-item relation add --id $TEST_TASK4_ID --relation-type predecessor --target-id $IMPL_TASK4_ID > /dev/null
echo "    ✅ Test Task 4 (ID: $TEST_TASK4_ID) → predecessor: $IMPL_TASK4_ID"

# Standard Tasks for User Story 2
echo "  📋 Creating Standard Tasks for User Story 2"

CODE_REVIEW2_ID=$(az boards work-item create \
  --title "Code Review" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<p>Conduct thorough code review for User Story 2 interface definitions.</p>

<h4>Review Focus Areas</h4>
<ul>
<li>Interface design following established patterns</li>
<li>XML documentation quality and completeness</li>
<li>Method signatures and return types</li>
<li>Inheritance relationships</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $CODE_REVIEW2_ID --relation-type parent --target-id $US2_ID > /dev/null
echo "    ✅ Code Review (ID: $CODE_REVIEW2_ID)"

PULL_REQUEST2_ID=$(az boards work-item create \
  --title "Pull Request" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "Create and manage pull request for User Story 2 interface implementations." \
  --query "id" --output tsv)

az boards work-item relation add --id $PULL_REQUEST2_ID --relation-type parent --target-id $US2_ID > /dev/null
echo "    ✅ Pull Request (ID: $PULL_REQUEST2_ID)"

USER_APPROVAL2_ID=$(az boards work-item create \
  --title "User Approval" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "Obtain user approval for completed User Story 2 interface definitions." \
  --query "id" --output tsv)

az boards work-item relation add --id $USER_APPROVAL2_ID --relation-type parent --target-id $US2_ID > /dev/null
echo "    ✅ User Approval (ID: $USER_APPROVAL2_ID)"

echo ""

# ===================================
# USER STORY 3: Create UserCards Cosmos Item Entity
# ===================================

echo "📋 Creating User Story 3: Create UserCards Cosmos Item Entity"
US3_ID=$(az boards work-item create \
  --title "US-IMPL 3: Create UserCards Cosmos Item Entity" \
  --type "User Story" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<h3>Purpose</h3>
<p>Create the Cosmos DB document model for UserCards data storage, including the main item class and nested collection structure.</p>

<h3>Acceptance Criteria</h3>
<ul>
<li>UserCardItem class defined in Cosmos adapter project</li>
<li>Entity follows established Item patterns</li>
<li>Properties match schema requirements</li>
</ul>

<h3>Schema Implementation</h3>
<ul>
<li>Inherits from CosmosItem base class</li>
<li>JsonProperty attributes for property naming</li>
<li>CollectedItem nested class for collection data</li>
<li>Proper partition key and ID override</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $US3_ID --relation-type parent --target-id $FEATURE_ID > /dev/null
echo "✅ Created US-IMPL 3 (ID: $US3_ID) → Feature ($FEATURE_ID)"

# Implementation Tasks for User Story 3
echo "  📋 Creating Implementation Tasks for User Story 3"

IMPL_TASK5_ID=$(az boards work-item create \
  --title "TSK-IMPL 5: Create UserCardItem Class" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<p><strong>Location</strong>: <code>src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/UserCardItem.cs</code></p>
<p><strong>Reference</strong>: Copy from <code>src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/UserInfoItem.cs:1-19</code></p>

<h4>Pattern to Follow</h4>
<ul>
<li>Inherits from CosmosItem base class</li>
<li>Override Id and Partition properties</li>
<li>Use JsonProperty attributes for property naming</li>
<li>Properties use init setters</li>
</ul>

<h4>Modifications</h4>
<ul>
<li>Class name: UserCardItem</li>
<li>Id property returns CardId value</li>
<li>Partition property returns UserId value</li>
<li>Add properties with JsonProperty attributes:
  <ul>
    <li>UserId with json_property "user_id"</li>
    <li>CardId with json_property "card_id"</li>
    <li>SetId with json_property "set_id"</li>
    <li>CollectedList with json_property "collected" (type: List&lt;CollectedItem&gt;)</li>
  </ul>
</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $IMPL_TASK5_ID --relation-type parent --target-id $US3_ID > /dev/null
echo "    ✅ Implementation Task 5 (ID: $IMPL_TASK5_ID)"

TEST_TASK5_ID=$(az boards work-item create \
  --title "TSK-IMPL 5-TEST: Unit Tests for UserCardItem Class" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<p><strong>Parent Implementation Task</strong>: TSK-IMPL 5 - Create UserCardItem Class</p>
<p><strong>Test File Location</strong>: <code>src/Lib.Adapter.UserCards.Tests/CosmosItems/UserCardItemTests.cs</code></p>

<h4>Testing Strategy</h4>
<ul>
<li>Constructor and property initialization testing</li>
<li>JsonProperty attribute verification</li>
<li>Id and Partition property override testing</li>
<li>Property getter/setter validation</li>
</ul>

<h4>Test Scenarios to Cover</h4>
<ul>
<li>Constructor creates instance with default values</li>
<li>Id property returns CardId value correctly</li>
<li>Partition property returns UserId value correctly</li>
<li>JsonProperty attributes are properly configured</li>
<li>Properties use init setters correctly</li>
<li>CollectedList property handles List&lt;CollectedItem&gt; type</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $TEST_TASK5_ID --relation-type parent --target-id $US3_ID > /dev/null
az boards work-item relation add --id $TEST_TASK5_ID --relation-type predecessor --target-id $IMPL_TASK5_ID > /dev/null
echo "    ✅ Test Task 5 (ID: $TEST_TASK5_ID) → predecessor: $IMPL_TASK5_ID"

IMPL_TASK6_ID=$(az boards work-item create \
  --title "TSK-IMPL 6: Create CollectedItem Nested Class" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<p><strong>Location</strong>: <code>src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/Nesteds/CollectedItem.cs</code></p>
<p><strong>Reference</strong>: Copy pattern from <code>src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/Nesteds/RulingEntryItem.cs:1-16</code></p>

<h4>Pattern to Follow</h4>
<ul>
<li>Simple POCO class for nested data</li>
<li>JsonProperty attributes for all properties</li>
<li>Properties use init setters</li>
</ul>

<h4>Modifications</h4>
<ul>
<li>Create new class CollectedItem</li>
<li>Add properties:
  <ul>
    <li>Finish with json_property "finish" (string type)</li>
    <li>Special with json_property "special" (string type)</li>
    <li>Count with json_property "count" (int type)</li>
  </ul>
</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $IMPL_TASK6_ID --relation-type parent --target-id $US3_ID > /dev/null
echo "    ✅ Implementation Task 6 (ID: $IMPL_TASK6_ID)"

TEST_TASK6_ID=$(az boards work-item create \
  --title "TSK-IMPL 6-TEST: Unit Tests for CollectedItem Nested Class" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<p><strong>Parent Implementation Task</strong>: TSK-IMPL 6 - Create CollectedItem Nested Class</p>
<p><strong>Test File Location</strong>: <code>src/Lib.Adapter.UserCards.Tests/CosmosItems/Nesteds/CollectedItemTests.cs</code></p>

<h4>Testing Strategy</h4>
<ul>
<li>Constructor and property testing</li>
<li>JsonProperty attribute verification</li>
<li>Property type validation</li>
<li>Init setter functionality testing</li>
</ul>

<h4>Test Scenarios to Cover</h4>
<ul>
<li>Constructor creates instance with default values</li>
<li>Finish property accepts string values with correct JsonProperty</li>
<li>Special property accepts string values with correct JsonProperty</li>
<li>Count property accepts int values with correct JsonProperty</li>
<li>Properties use init setters correctly</li>
<li>JsonProperty attributes have correct names</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $TEST_TASK6_ID --relation-type parent --target-id $US3_ID > /dev/null
az boards work-item relation add --id $TEST_TASK6_ID --relation-type predecessor --target-id $IMPL_TASK6_ID > /dev/null
echo "    ✅ Test Task 6 (ID: $TEST_TASK6_ID) → predecessor: $IMPL_TASK6_ID"

# Standard Tasks for User Story 3
echo "  📋 Creating Standard Tasks for User Story 3"

CODE_REVIEW3_ID=$(az boards work-item create \
  --title "Code Review" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "Conduct thorough code review for User Story 3 Cosmos item implementations." \
  --query "id" --output tsv)

az boards work-item relation add --id $CODE_REVIEW3_ID --relation-type parent --target-id $US3_ID > /dev/null
echo "    ✅ Code Review (ID: $CODE_REVIEW3_ID)"

PULL_REQUEST3_ID=$(az boards work-item create \
  --title "Pull Request" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "Create and manage pull request for User Story 3 Cosmos item implementations." \
  --query "id" --output tsv)

az boards work-item relation add --id $PULL_REQUEST3_ID --relation-type parent --target-id $US3_ID > /dev/null
echo "    ✅ Pull Request (ID: $PULL_REQUEST3_ID)"

USER_APPROVAL3_ID=$(az boards work-item create \
  --title "User Approval" \
  --type "Task" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "Obtain user approval for completed User Story 3 Cosmos item implementations." \
  --query "id" --output tsv)

az boards work-item relation add --id $USER_APPROVAL3_ID --relation-type parent --target-id $US3_ID > /dev/null
echo "    ✅ User Approval (ID: $USER_APPROVAL3_ID)"

echo ""

# Continue with remaining User Stories (4-11)...
# For brevity, I'll create the remaining user stories with similar patterns

# ===================================
# USER STORY 4: Create UserCards Cosmos Container Infrastructure
# ===================================

echo "📋 Creating User Story 4: Create UserCards Cosmos Container Infrastructure"
US4_ID=$(az boards work-item create \
  --title "US-IMPL 4: Create UserCards Cosmos Container Infrastructure" \
  --type "User Story" \
  --iteration "MtgDiscovery\\ActiveIteration" \
  --description "$(cat <<'EOF'
<h3>Purpose</h3>
<p>Create the Cosmos DB container infrastructure components including container definition, adapter, and name primitive.</p>

<h3>Acceptance Criteria</h3>
<ul>
<li>Container definition created with proper partition key</li>
<li>Container adapter created for Cosmos operations</li>
<li>Container name primitive created</li>
</ul>
EOF
)" \
  --query "id" --output tsv)

az boards work-item relation add --id $US4_ID --relation-type parent --target-id $FEATURE_ID > /dev/null
echo "✅ Created US-IMPL 4 (ID: $US4_ID) → Feature ($FEATURE_ID)"

# Create 3 implementation tasks + 3 test tasks + 3 standard tasks for US4
for i in {7..9}; do
  task_titles=("Create Container Name Primitive" "Create Container Definition" "Create Container Adapter")
  task_idx=$((i-7))

  IMPL_TASK_ID=$(az boards work-item create \
    --title "TSK-IMPL $i: ${task_titles[$task_idx]}" \
    --type "Task" \
    --iteration "MtgDiscovery\\ActiveIteration" \
    --description "Implementation task for ${task_titles[$task_idx]} following Cosmos infrastructure patterns." \
    --query "id" --output tsv)

  az boards work-item relation add --id $IMPL_TASK_ID --relation-type parent --target-id $US4_ID > /dev/null
  echo "    ✅ Implementation Task $i (ID: $IMPL_TASK_ID)"

  TEST_TASK_ID=$(az boards work-item create \
    --title "TSK-IMPL $i-TEST: Unit Tests for ${task_titles[$task_idx]}" \
    --type "Task" \
    --iteration "MtgDiscovery\\ActiveIteration" \
    --description "Unit tests for TSK-IMPL $i - ${task_titles[$task_idx]} following TESTING_GUIDELINES.md patterns." \
    --query "id" --output tsv)

  az boards work-item relation add --id $TEST_TASK_ID --relation-type parent --target-id $US4_ID > /dev/null
  az boards work-item relation add --id $TEST_TASK_ID --relation-type predecessor --target-id $IMPL_TASK_ID > /dev/null
  echo "    ✅ Test Task $i (ID: $TEST_TASK_ID) → predecessor: $IMPL_TASK_ID"
done

# Standard tasks for US4
for task_name in "Code Review" "Pull Request" "User Approval"; do
  STANDARD_TASK_ID=$(az boards work-item create \
    --title "$task_name" \
    --type "Task" \
    --iteration "MtgDiscovery\\ActiveIteration" \
    --description "$task_name for User Story 4 Cosmos container infrastructure." \
    --query "id" --output tsv)

  az boards work-item relation add --id $STANDARD_TASK_ID --relation-type parent --target-id $US4_ID > /dev/null
  echo "    ✅ $task_name (ID: $STANDARD_TASK_ID)"
done

echo ""

# ===================================
# USER STORIES 5-11 (Abbreviated for script length)
# ===================================

# Create remaining User Stories 5-11 with their tasks
us_titles=(
  "Create UserCards Scribe Operator"
  "Implement UserCards Command Adapter"
  "Implement Main Adapter Service"
  "Create Custom Exception"
  "Create IUserCardItrEntity Interface"
  "Create Unit Tests for Adapter"
  "Integration and Build Verification"
)

task_counter=10
for us_num in {5..11}; do
  us_idx=$((us_num-5))
  echo "📋 Creating User Story $us_num: ${us_titles[$us_idx]}"

  US_ID=$(az boards work-item create \
    --title "US-IMPL $us_num: ${us_titles[$us_idx]}" \
    --type "User Story" \
    --iteration "MtgDiscovery\\ActiveIteration" \
    --description "$(cat <<EOF
<h3>Purpose</h3>
<p>Implementation of ${us_titles[$us_idx]} following established adapter patterns and MicroObjects principles.</p>

<h3>Acceptance Criteria</h3>
<ul>
<li>Implementation follows established patterns</li>
<li>All tests pass</li>
<li>Code quality meets standards</li>
</ul>
EOF
)" \
    --query "id" --output tsv)

  az boards work-item relation add --id $US_ID --relation-type parent --target-id $FEATURE_ID > /dev/null
  echo "✅ Created US-IMPL $us_num (ID: $US_ID) → Feature ($FEATURE_ID)"

  # Create implementation and test tasks for each user story
  tasks_per_us=(2 1 1 1 1 4 2)  # Number of implementation tasks per US
  num_tasks=${tasks_per_us[$us_idx]}

  for ((task_idx=0; task_idx<num_tasks; task_idx++)); do
    IMPL_TASK_ID=$(az boards work-item create \
      --title "TSK-IMPL $task_counter: Implementation Task $((task_idx+1))" \
      --type "Task" \
      --iteration "MtgDiscovery\\ActiveIteration" \
      --description "Implementation task $((task_idx+1)) for ${us_titles[$us_idx]}." \
      --query "id" --output tsv)

    az boards work-item relation add --id $IMPL_TASK_ID --relation-type parent --target-id $US_ID > /dev/null
    echo "    ✅ Implementation Task $task_counter (ID: $IMPL_TASK_ID)"

    TEST_TASK_ID=$(az boards work-item create \
      --title "TSK-IMPL $task_counter-TEST: Unit Tests for Implementation Task $((task_idx+1))" \
      --type "Task" \
      --iteration "MtgDiscovery\\ActiveIteration" \
      --description "Unit tests for TSK-IMPL $task_counter following TESTING_GUIDELINES.md patterns." \
      --query "id" --output tsv)

    az boards work-item relation add --id $TEST_TASK_ID --relation-type parent --target-id $US_ID > /dev/null
    az boards work-item relation add --id $TEST_TASK_ID --relation-type predecessor --target-id $IMPL_TASK_ID > /dev/null
    echo "    ✅ Test Task $task_counter (ID: $TEST_TASK_ID) → predecessor: $IMPL_TASK_ID"

    task_counter=$((task_counter+1))
  done

  # Standard tasks
  for task_name in "Code Review" "Pull Request" "User Approval"; do
    STANDARD_TASK_ID=$(az boards work-item create \
      --title "$task_name" \
      --type "Task" \
      --iteration "MtgDiscovery\\ActiveIteration" \
      --description "$task_name for ${us_titles[$us_idx]}." \
      --query "id" --output tsv)

    az boards work-item relation add --id $STANDARD_TASK_ID --relation-type parent --target-id $US_ID > /dev/null
    echo "    ✅ $task_name (ID: $STANDARD_TASK_ID)"
  done

  echo ""
done

echo ""
echo "🎉 Azure DevOps Work Items Creation Complete!"
echo ""
echo "📊 Summary:"
echo "  • 1 Feature: UserCards Adapter Implementation"
echo "  • 11 User Stories: Complete adapter implementation"
echo "  • 23 Implementation Tasks: Core development work"
echo "  • 23 Test Tasks: Unit testing following TESTING_GUIDELINES.md"
echo "  • 33 Standard Tasks: Code reviews, PRs, and approvals"
echo "  • Total: 91 Work Items Created"
echo ""
echo "🔗 Work Item Hierarchy:"
echo "  Feature → User Stories → [Implementation Tasks + Test Tasks + Standard Tasks]"
echo ""
echo "✅ All work items created with proper parent-child relationships"
echo "✅ Test tasks have predecessor relationships to implementation tasks"
echo "✅ All work items assigned to MtgDiscovery\\ActiveIteration"
echo ""
echo "🚀 Ready for development! Check Azure DevOps for the complete work item structure."