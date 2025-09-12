# Pull Request - Ready for Review

## 📊 PR Summary
<!-- Brief description of changes made -->
**Changes Made:**
- 

**Story/Work Item:** #[work-item-number]
**Branch:** `[feature/branch-name]`

---

## ✅ Pre-Merge Validation Checklist
*This checklist will be validated by quinn-pr-finalizer before merge approval.*

### 🏗️ Build & Compilation
- [ ] Solution builds without errors
- [ ] Solution builds without warnings  
- [ ] All projects compile successfully
- [ ] No build configuration issues
- [ ] Dependencies resolve correctly

### 📝 Code Quality & Standards
- [ ] TODOs properly formatted (`// TODO: [Name] [YYYY-MM-DD] - Description`)
- [ ] No unused using statements
- [ ] #pragma directives have justification comments
- [ ] .editorconfig compliance verified
- [ ] No commented-out code blocks
- [ ] No debug/console statements in production code
- [ ] No hardcoded values (connection strings, secrets, magic numbers)
- [ ] Naming conventions followed

### 🧪 Testing & Coverage  
- [ ] All tests pass (`dotnet test` success)
- [ ] New code has corresponding tests
- [ ] No ignored tests without justification
- [ ] Test naming follows `MethodName_Scenario_ExpectedBehavior` convention
- [ ] Test methods are isolated (no dependencies)
- [ ] Integration tests updated if needed

### 🏛️ Architecture & Design
- [ ] MicroObjects patterns followed
- [ ] Layer boundaries respected (Entry → Domain → Aggregator → Adapter)  
- [ ] Public methods have XML documentation
- [ ] New public classes have corresponding interfaces
- [ ] Constructor injection used (no static dependencies)
- [ ] Exception handling follows project patterns

### 🔐 Security & Performance
- [ ] No secrets, API keys, or passwords in code
- [ ] Input validation on public methods
- [ ] SQL injection prevention (parameterized queries)
- [ ] `ConfigureAwait(false)` on all async calls
- [ ] Proper disposal of resources (`using` statements)
- [ ] No obvious resource leaks

### 📚 Documentation & Communication
- [ ] CLAUDE.md updated (if architectural changes)
- [ ] Public APIs documented with XML comments
- [ ] Breaking changes documented
- [ ] Database migration scripts provided (if needed)
- [ ] New configuration settings documented

### 📦 Dependencies & Source Control  
- [ ] No merge conflicts with target branch
- [ ] Dependencies are appropriate and justified
- [ ] Package versions current (latest stable)
- [ ] No circular project references
- [ ] Commit messages follow conventions
- [ ] No unnecessary files committed (bin/, obj/, IDE files)

---

## 🤖 Automated Validation Results
*This section will be updated by quinn-pr-finalizer*

### ✅ Validation Summary
- **Status**: ⏳ Pending Validation
- **Build Status**: ⏳ Not Tested
- **Test Results**: ⏳ Not Run
- **Code Analysis**: ⏳ Not Performed

### 📋 Detailed Results
<!-- Quinn-PR-Finalizer will update this section with detailed validation results -->

---

## 🔍 Manual Review Items
*Items requiring human judgment - not automated*

- [ ] **Business Logic Correctness** - Implementation meets requirements
- [ ] **User Experience Impact** - Changes don't negatively affect UX  
- [ ] **Performance Impact** - No significant performance degradation
- [ ] **Backward Compatibility** - Changes don't break existing functionality
- [ ] **Error Handling** - Appropriate error messages and user feedback
- [ ] **Security Review** - Changes don't introduce security vulnerabilities

---

## 🚨 Known Issues / Exceptions
*Document any validation exceptions or known issues*

<!-- If any checklist items cannot be completed, explain why here -->

---

## 📝 Testing Instructions
*How to test these changes*

### Prerequisites:
- 

### Test Steps:
1. 
2. 
3. 

### Expected Results:
- 

---

## 🔄 Post-Merge Actions
*Actions to be taken after merge*

- [ ] Deploy to staging environment
- [ ] Update documentation
- [ ] Notify stakeholders
- [ ] Monitor for issues

---

## 📞 Reviewer Notes
*Additional context for reviewers*

<!-- Any special considerations, architectural decisions, or trade-offs made -->

---

*This PR template ensures comprehensive validation before merge. All automated checks must pass before human review.*