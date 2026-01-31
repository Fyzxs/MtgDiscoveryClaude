# MicroObjects Philosophy

This codebase uses MicroObjects—a design philosophy where every concept is explicitly represented as an object.

## Core Principle

**Have a representation for every concept.** If you can name it, make it an object. This creates code that is:
- Self-documenting through meaningful types
- Highly maintainable through low coupling
- Extremely flexible through composition
- Nearly bug-free through simplicity

## The Philosophy

- **Trust collaborators** — Objects behave as designed
- **Minimal knowledge** — Each class knows only what it needs
- **Single responsibility** — One thing, done well
- **Thoughts as types** — Explicit over implicit
- **Continuous refinement** — Refactor until the code is obvious
- **Code-free constructors** — Only assign dependencies, never initialize logic

## Red Flags

Watch for these signals that a design needs rethinking:

- Classes with more than 5-7 methods (sign of multiple responsibilities)
- Methods with more than 3-4 lines (overly complex logic)
- Type checking or inspection (`typeof`, `instanceof`)
- Mutable state (prefer immutability)
- Static methods or properties
- Utility/Manager/Service/Helper classes (symptoms of unclear responsibility)

**When you see these patterns in the codebase, look at how they're handled.** Understanding the *why* matters more than following rules.

## Learning the Style

The best way to learn this style is to read the existing code. Look for:
- How objects expose behavior instead of data
- How conditional logic is replaced with polymorphism
- How primitives are wrapped in domain objects
- How external dependencies are abstracted
- How constructors stay simple and dependency-focused

See your team lead or code examples for real patterns in action.