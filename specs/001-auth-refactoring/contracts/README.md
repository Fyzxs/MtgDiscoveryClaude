# API Contracts: Authentication Flow Refactoring

**Feature**: Authentication Flow Refactoring
**Branch**: 001-auth-refactoring
**Date**: 2026-01-17
**Constitution Version**: 1.0.0

## Overview

This directory contains the TypeScript interface contracts for all components, hooks, and utilities in the authentication refactoring. These contracts define the public APIs that will be implemented in Phase 2 (Implementation).

## Contract Files

1. **[utility-contracts.ts](./utility-contracts.ts)** - Storage utility functions
2. **[hook-contracts.ts](./hook-contracts.ts)** - Custom React hooks
3. **[component-contracts.ts](./component-contracts.ts)** - React component props
4. **[apollo-contracts.ts](./apollo-contracts.ts)** - Apollo Client configuration updates

## Constitution Alignment

All contracts follow:
- ✅ **Principle I** (MicroObjects → TypeScript): Explicit interface for every concept
- ✅ **Principle IV** (Null Boundary Guards): Return `null` at boundaries, type guards for validation
- ✅ **Principle VI** (Code Style): TypeScript interfaces, immutable patterns
- ✅ Frontend Standards: React 19 patterns, Material-UI integration

## Usage

These contracts serve as:
1. **Implementation Guide**: Define exact function signatures and return types
2. **Test Specifications**: Define what to test for each function/component
3. **Documentation**: Self-documenting via TypeScript types
4. **Contract Tests**: Can be validated with TypeScript compiler

## Dependencies

All contracts assume:
- React 19 with TypeScript
- Auth0 React SDK (`@auth0/auth0-react`)
- Apollo Client
- React Router DOM
- Material-UI