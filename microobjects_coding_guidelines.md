# MicroObjects Coding Guidelines

## Driving Principle
> **Have a representation for every concept that exists in the code**

Every abstract idea, behavior, or data element should be represented as a distinct object. No concept should exist implicitly or be scattered across multiple locations.

## Core Technical Practices

### 1. No Getters or Setters
Objects expose behavior, not data.

```csharp
// ❌ BAD - Exposing data
public class Cash {
    public int Dollars { get; }
}

// ✅ GOOD - Exposing behavior
public class Cash {
    public bool CanCover(ICash amount) { ... }
    public ICash Subtract(ICash amount) { ... }
}
```

**Guideline**: Ask objects "What can you do?" not "What data do you have?"

### 2. Be Immutable
Objects should not change state after construction.

```csharp
public class Account {
    private readonly IAccountId _id;
    private readonly IBalance _balance;
    
    public Account(IAccountId id, IBalance balance) {
        _id = id;
        _balance = balance;
    }
    
    public IAccount Deposit(IMoney amount) {
        return new Account(_id, _balance.Add(amount));
    }
}
```

**Guideline**: Use `private readonly` fields. Return new instances for state changes.

### 3. Interface for Behavior Contracts
Always program to interfaces, never to concrete types.

```csharp
// ❌ BAD - Using concrete type
ConcreteValidator validator = new ConcreteValidator();

// ✅ GOOD - Using interface
IValidator validator = new ConcreteValidator();
```

**Guideline**: Create a 1:1 interface for every class. The interface defines the behavior contract.

### 4. Abstract 3rd Party Code
Wrap external dependencies in your own abstractions.

```csharp
// ❌ BAD - Direct dependency on HttpClient
public class UserService {
    public async Task<User> GetUser(string id) {
        var client = new HttpClient();
        var response = await client.GetAsync($"api/users/{id}");
        return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
    }
}

// ✅ GOOD - Abstracted dependency
public interface IUserApi {
    Task<IUser> GetUser(IUserId id);
}

public class UserService {
    private readonly IUserApi _api;
    
    public UserService(IUserApi api) {
        _api = api;
    }
    
    public async Task<IUser> GetUser(IUserId id) {
        return await _api.GetUser(id);
    }
}
```

### 5. No Public Statics
Static methods and properties violate object-oriented principles.

```csharp
// ❌ BAD - Static utility
public static class DateUtils {
    public static string Format(DateTime date) { ... }
}

// ✅ GOOD - Instance behavior
public interface IDateFormatter {
    string Format(IDate date);
}
```

### 6. If Only as Guard Clauses
Use if statements only for early returns, not for branching logic.

```csharp
// ❌ BAD - Branching logic
public string Process(int value) {
    string result = "";
    if (value % 3 == 0) result += "Fizz";
    if (value % 5 == 0) result += "Buzz";
    return result == "" ? value.ToString() : result;
}

// ✅ GOOD - Guard clauses
public string Process(int value) {
    if (IsFizzBuzz(value)) return "FizzBuzz";
    if (IsFizz(value)) return "Fizz";
    if (IsBuzz(value)) return "Buzz";
    return value.ToString();
}
```

### 7. Switch and Else Always Evil
Replace switch statements and else clauses with polymorphism.

```csharp
// ❌ BAD - Switch statement
switch(user.Type()) {
    case UserType.Admin: return adminPrivileges();
    case UserType.Regular: return regularPrivileges();
    case UserType.Guest: return guestPrivileges();
}

// ✅ GOOD - Polymorphic objects
interface IUser { 
    IPrivileges Privileges(); 
}

class AdminUser : IUser { 
    public IPrivileges Privileges() => new AdminPrivileges(); 
}
```

### 8. No Nulls
Use Null Object pattern instead of null references.

```csharp
// ❌ BAD - Returning null
public User FindUser(string id) {
    if (!Exists(id)) return null;
    return LoadUser(id);
}

// ✅ GOOD - Null Object
public IUser FindUser(IUserId id) {
    if (Exists(id) is false) return new UnknownUser();
    return LoadUser(id);
}

// Null Object implementation
public class UnknownUser : IUser {
    public string Name() => "Unknown";
    public void SendEmail(IEmail email) { /* no-op */ }
}
```

**Important**: Null Objects should NOT have `isNull()` methods. They should provide sensible default behaviors.

### 9. No New Inline
Never instantiate objects within method bodies.

```csharp
// ❌ BAD - Inline instantiation
public void ProcessOrder(Order order) {
    var emailer = new EmailService();
    emailer.Send(order.GetCustomerEmail());
}

// ✅ GOOD - Dependency injection
public void ProcessOrder(IOrder order, IEmailService emailer) {
    emailer.Send(order.CustomerEmail());
}
```

### 10. Extract Cohesion
Create objects to represent relationships between concepts.

```csharp
// ❌ BAD - Low cohesion
public class Order {
    public void ApplyDiscount(decimal percentage, Customer customer) {
        if (customer.IsVip && 0.2m < percentage) {
            // apply special VIP discount logic
        }
    }
}

// ✅ GOOD - High cohesion
public interface IDiscountPolicy {
    IDiscount CalculateFor(ICustomer customer);
}

public class VipDiscountPolicy : IDiscountPolicy {
    public IDiscount CalculateFor(ICustomer customer) {
        return new VipDiscount(customer);
    }
}
```

### 11. Composition Over Inheritance
Prefer composition to create flexible designs.

```java
// ❌ BAD - Inheritance hierarchy
abstract class Animal {
    abstract void makeSound();
}
class Dog extends Animal {
    void makeSound() { bark(); }
}

// ✅ GOOD - Composition
interface ISoundMaker {
    void makeSound();
}
class Dog {
    private final ISoundMaker soundMaker;
    
    public Dog(ISoundMaker soundMaker) {
        this.soundMaker = soundMaker;
    }
}
```

**Rule of Three**: Only create abstractions when you have three concrete implementations.

### 12. No Primitives
Wrap primitives in domain-specific objects.

```csharp
// ❌ BAD - Primitive obsession
public void Transfer(string fromAccount, string toAccount, decimal amount) { }

// ✅ GOOD - Domain objects
public void Transfer(IAccountNumber from, IAccountNumber to, IMoney amount) { }

// Implementation with implicit operators
public class AccountNumber : IAccountNumber {
    private readonly string _value;
    
    public AccountNumber(string value) {
        if (IsValid(value) is false) throw new ArgumentException();
        _value = value;
    }
    
    public static implicit operator string(AccountNumber number) => number._value;
    public static implicit operator AccountNumber(string value) => new AccountNumber(value);
}
```

**Note**: Collections are primitives too - wrap them in domain-specific objects.

### 13. No Enums
Replace enums with polymorphic objects.

```csharp
// ❌ BAD - Enum with switch
public enum Status { Active, Inactive, Pending }

// ✅ GOOD - Polymorphic status objects
public interface IStatus {
    bool CanProcess();
    IStatus TransitionTo(IAction action);
}

public class ActiveStatus : IStatus {
    public bool CanProcess() => true;
    public IStatus TransitionTo(IAction action) => action.Execute(this);
}
```

### 14. No Logic in Constructors
Constructors should only assign dependencies to fields.

```csharp
// ❌ BAD - Logic in constructor
public class UserService {
    public UserService(string connectionString) {
        _connection = new SqlConnection(connectionString);
        _connection.Open(); // Logic!
        ValidateConnection(); // Logic!
    }
}

// ✅ GOOD - Logic-free constructor
public class UserService {
    private readonly IDbConnection _connection;
    
    public UserService(IDbConnection connection) {
        _connection = connection;
    }
}
```

### 15. Never Reflection
Reflection breaks encapsulation and makes code fragile.

```csharp
// ❌ BAD - Using reflection
var method = obj.GetType().GetMethod("DoSomething");
method.Invoke(obj, null);

// ✅ GOOD - Using interfaces
IDoer doer = obj;
doer.DoSomething();
```

### 16. Never Type Inspection
No instanceof, typeof, or type checking.

```csharp
// ❌ BAD - Type checking
if (animal is Dog) {
    ((Dog)animal).Bark();
} else if (animal is Cat) {
    ((Cat)animal).Meow();
}

// ✅ GOOD - Polymorphism
animal.MakeSound();
```

### 17. No Greater Than Comparisons
Always use less than (<) for consistency and clarity.

```csharp
// ❌ BAD - Using greater than
if (age > 18) { ... }
if (score >= 90) { ... }
if (x > 5 && x < 10) { ... }

// ✅ GOOD - Using less than consistently
if (18 < age) { ... }
if (89 < score) { ... }
if (5 < x && x < 10) { ... }  // Reads like a number line

// For ranges
// ❌ BAD - Mixed operators
if (x < 5 || x > 10) { ... }

// ✅ GOOD - Consistent operators
if (x < 5 || 10 < x) { ... }  // Outside range
if (5 < x && x < 10) { ... }  // Inside range
```

**Rationale**: Using only `<` reduces complexity, matches number line representation, and prevents logical errors.

### 18. No Boolean Negation
Avoid the `!` operator. Create explicit inverse methods instead.

```csharp
// ❌ BAD - Using negation operator
if (!user.IsValid()) { ... }
if (!string.IsNullOrEmpty(name)) { ... }

// ✅ GOOD - Explicit inverse methods
if (user.IsNotValid()) { ... }
if (user.IsInvalid()) { ... }

// When you control the interface
public interface IValidatable {
    bool IsValid();
    bool IsNotValid();  // Explicitly defined
}

// When you don't control the source
if (string.IsNullOrEmpty(name) is false) { ... }
if (externalApi.HasPermission() is false) { ... }
```

**Rationale**: Explicit inverse methods and `is false` improve readability significantly over the `!` operator.

## Implementation Patterns

### Creating New Objects
1. Define the interface first (behavior contract)
2. Create the concrete implementation
3. Use constructor injection for all dependencies
4. Keep constructors logic-free
5. Make all fields private readonly

### Refactoring Existing Code
1. Identify implicit concepts (look for related data and behavior)
2. Extract these into explicit objects
3. Replace conditionals with polymorphism
4. Wrap primitives in domain objects
5. Abstract external dependencies

### Testing Guidelines
1. Test through interfaces, not concrete classes
2. Use real objects instead of mocks when possible
3. Create simple test implementations of interfaces
4. Each test should verify one behavior
5. Constructor injection enables easy testing

## Benefits of MicroObjects

### Technical Benefits
- **Thread Safety**: Immutable objects are inherently thread-safe
- **Testability**: Constructor injection and single responsibility
- **Maintainability**: Clear responsibilities and minimal coupling
- **Flexibility**: Rigid components enable flexible composition

### Design Benefits
- **Clarity**: Every concept has explicit representation
- **Simplicity**: Small, focused objects with single responsibilities
- **Discoverability**: Clear interfaces show available behaviors
- **Evolution**: Easy to add new concepts without modifying existing ones

## Red Flags to Avoid
- Methods returning void (indicates side effects)
- Classes with more than 5-7 methods
- Methods with more than 3-4 lines
- Any use of `instanceof` or type checking
- Mutable state
- Static methods or properties
- Data classes (classes with only getters/setters)
- Utility classes
- Manager/Service/Helper class names
- Greater than operators (>, >=)
- Boolean negation operator (!)

## Key Philosophies
- **Trust your collaborators** to behave properly
- **Make each class as "ignorant" as possible** - classes should know minimal details about their collaborators
- **Objects should do one thing well** - single responsibility at its extreme
- **Represent what you're thinking through types** - if you can name it, make it an object
- **When do you stop refactoring? Until you can't refactor** - continuously break down into smaller units
- **Leave constructors code-free** for more configurable, transparent, guaranteed, and maintainable classes

## CLEAN Code Principles
MicroObjects embody the CLEAN principles:

- **Cohesive**: All parts of an object work together for a single purpose
- **Loosely Coupled**: Minimal dependencies between classes
- **Encapsulated**: Hidden implementation details, exposed behavior
- **Assertive**: Objects manage their own state and enforce their own rules
- **Nonredundant**: No duplicate code or concepts in the system

## Summary
MicroObjects is about creating a rich domain model where every concept is explicitly represented. By following these practices, you create code that is:
- Self-documenting through meaningful types
- Highly maintainable through low coupling
- Extremely flexible through composition
- Nearly bug-free through simplicity

Remember: **If you can name it, make it an object.**