# MicroObjects - Bulletpoint Summary

## Core Principle
- **Have a representation for every concept** - If you can name it, make it an object

## 18 Technical Practices

### Object Design
- **No Getters/Setters** - Expose behavior, not data
- **Be Immutable** - Use `private readonly` fields, return new instances for changes
- **Interface Everything** - 1:1 interface for every class
- **No Logic in Constructors** - Only assign dependencies to fields

### Dependencies & Coupling
- **Abstract 3rd Party Code** - Wrap external dependencies
- **No Public Statics** - Use instance methods instead
- **No New Inline** - Use dependency injection, never instantiate in methods
- **Composition Over Inheritance** - Prefer composition for flexibility

### Control Flow
- **If Only as Guard Clauses** - Early returns only, no branching
- **No Switch/Else** - Replace with polymorphism
- **No Greater Than** - Use only `<` operator for consistency
- **No Boolean Negation** - Use `is false` or explicit inverse methods

### Type Safety
- **No Nulls** - Use Null Object pattern
- **No Primitives** - Wrap in domain objects
- **No Enums** - Replace with polymorphic objects
- **Never Reflection** - Breaks encapsulation
- **No Type Inspection** - No instanceof/typeof checks

### Cohesion
- **Extract Cohesion** - Create objects for concept relationships

## Key Philosophies
- **Trust collaborators** to behave properly
- **Make classes ignorant** - Minimal knowledge of collaborators
- **Single responsibility extreme** - One thing done well
- **Represent thoughts as types** - Explicit over implicit
- **Refactor until impossible** - Continuous breakdown
- **Code-free constructors** - More maintainable and testable

## CLEAN Principles
- **Cohesive** - Parts work together for single purpose
- **Loosely Coupled** - Minimal dependencies
- **Encapsulated** - Hidden implementation
- **Assertive** - Objects manage own state/rules
- **Nonredundant** - No duplicate code/concepts

## Red Flags to Avoid
- Void methods (side effects)
- Classes with >5-7 methods
- Methods with >3-4 lines
- Type checking/instanceof
- Mutable state
- Static methods/properties
- Data classes (only getters/setters)
- Utility/Manager/Service/Helper classes
- Greater than operators
- Boolean negation operator

## Implementation Patterns

### Creating Objects
1. Define interface first (behavior contract)
2. Create concrete implementation
3. Use constructor injection
4. Keep constructors logic-free
5. Make fields private readonly

### Refactoring Process
1. Identify implicit concepts
2. Extract into explicit objects
3. Replace conditionals with polymorphism
4. Wrap primitives in domain objects
5. Abstract external dependencies

### Testing Approach
1. Test through interfaces
2. Use real objects over mocks
3. Create simple test implementations
4. One behavior per test
5. Constructor injection enables testing

## Benefits

### Technical
- **Thread Safety** - Immutability
- **Testability** - Constructor injection, single responsibility
- **Maintainability** - Clear responsibilities, minimal coupling
- **Flexibility** - Rigid components, flexible composition

### Design
- **Clarity** - Explicit concepts
- **Simplicity** - Small, focused objects
- **Discoverability** - Clear interfaces
- **Evolution** - Easy extension without modification

## The Result
Code that is:
- Self-documenting through types
- Highly maintainable via low coupling
- Extremely flexible via composition
- Nearly bug-free via simplicity