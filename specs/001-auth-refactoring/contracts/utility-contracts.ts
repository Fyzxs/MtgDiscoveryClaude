/**
 * Storage Utility Contracts
 *
 * File: client/src/utils/userStorage.ts
 * Phase: 1 (Quick Win - localStorage detection)
 *
 * Constitution Alignment:
 * - Principle I: Explicit types for every concept (StoredUserData interface)
 * - Principle IV: Boundary validation (returns null at boundary, validates inside)
 * - Immutability: Functions return new objects, never mutate
 */

// ============================================================================
// Types
// ============================================================================

/**
 * User session data stored in localStorage for returning user detection.
 *
 * Immutable: Once created, only lastLoginAt is updated via new object creation.
 * Boundary: This type crosses the localStorage boundary (serialized to JSON).
 */
export interface StoredUserData {
  // Auth0 Identity (required)
  sub: string;              // Auth0 subject identifier (e.g., "auth0|123456")
  userId: string;           // Backend user ID (UUID format)

  // Auth0 Profile (optional)
  email?: string;           // User email from Auth0
  name?: string;            // Display name from Auth0
  picture?: string;         // Avatar URL from Auth0

  // Backend Data (optional)
  displayName?: string;     // Backend-chosen display name

  // Session Tracking (required)
  registeredAt: string;     // ISO 8601 timestamp of first registration
  lastLoginAt: string;      // ISO 8601 timestamp of most recent login
}

// ============================================================================
// Storage Operations
// ============================================================================

/**
 * Retrieves stored user data from localStorage.
 *
 * Boundary Validation:
 * - Returns null if no data exists
 * - Returns null if data is invalid (corrupted, missing fields, wrong types)
 * - Validates using type guard before returning
 * - Clears corrupted data automatically
 *
 * @returns Validated user data or null
 *
 * Constitution:
 * - Principle IV: Validates at boundary, interior code assumes valid
 * - TypeScript strict null checks prevent undefined access
 */
export function getStoredUserData(): StoredUserData | null;

/**
 * Type guard for runtime validation of stored user data.
 *
 * Validates:
 * - Required fields: sub, userId, registeredAt, lastLoginAt
 * - Optional fields: email, name, picture, displayName (if present)
 * - Field types match interface
 * - Timestamps are valid ISO 8601 format
 *
 * @param data - Unknown data from localStorage
 * @returns Type predicate for TypeScript
 *
 * Constitution:
 * - Principle IV: Boundary guard prevents invalid data entering system
 */
export function isValidStoredUserData(data: unknown): data is StoredUserData;

/**
 * Checks if a user with the given Auth0 sub is a returning user.
 *
 * Logic:
 * 1. Get stored user data (returns null if invalid/missing)
 * 2. Compare stored sub with provided sub
 * 3. If mismatch (different user), clear old data and return false
 * 4. If match, return true
 *
 * @param auth0Sub - Auth0 subject identifier to check
 * @returns True if returning user with matching sub, false otherwise
 *
 * Constitution:
 * - Principle IV: Relies on getStoredUserData's boundary validation
 * - Cleans up on sub mismatch (data integrity)
 */
export function isReturningUser(auth0Sub: string): boolean;

/**
 * Saves user data to localStorage after successful registration.
 *
 * Serialization:
 * - Converts StoredUserData to JSON
 * - Stores in localStorage['mtg-user-data']
 * - Handles quota exceeded errors gracefully
 *
 * @param data - Validated user data to store
 *
 * Constitution:
 * - Assumes data is already validated (called after registration success)
 * - Immutable: Does not modify input parameter
 */
export function saveUserData(data: StoredUserData): void;

/**
 * Updates the last login timestamp for returning users.
 *
 * Immutability:
 * - Retrieves stored data
 * - Creates NEW object with updated lastLoginAt
 * - Saves new object (does not mutate original)
 *
 * Safety:
 * - Returns early if no stored data exists
 * - Does not throw errors
 *
 * @returns void
 *
 * Constitution:
 * - Immutable update pattern: {...stored, lastLoginAt: newDate}
 */
export function updateLastLogin(): void;

/**
 * Clears stored user data from localStorage.
 *
 * Usage:
 * - Called on user logout
 * - Called when corrupted data detected
 * - Called on sub mismatch
 *
 * @returns void
 *
 * Constitution:
 * - Simple, single-purpose function
 */
export function clearUserData(): void;

// ============================================================================
// Helper Functions (Internal)
// ============================================================================

/**
 * Validates ISO 8601 date string format.
 *
 * Checks:
 * - String can be parsed as Date
 * - Resulting date is valid (not NaN)
 * - String matches ISO format when re-serialized
 *
 * @param dateString - String to validate
 * @returns True if valid ISO 8601 date
 *
 * Note: May be internal (not exported) depending on implementation
 */
export function isValidISODate(dateString: string): boolean;

// ============================================================================
// Constants
// ============================================================================

/**
 * localStorage key for user data.
 *
 * Current: 'mtg-user-data'
 * Phase 5: Migrates to STORAGE_KEYS.USER_DATA constant
 */
export const USER_STORAGE_KEY: string = 'mtg-user-data';

// ============================================================================
// Storage Keys (Phase 5)
// ============================================================================

/**
 * File: client/src/utils/storageKeys.ts
 * Phase: 5 (Storage Cleanup)
 *
 * Centralizes all localStorage keys for consistency.
 */
export const STORAGE_KEYS = {
  USER_DATA: 'mtg-user-data',
  LANGUAGE: 'mtg-discovery-language',
  VISITED: 'mtg-discovery-visited',
  CARD_SIZE: 'mtg-card-size-preference',
} as const;

// Type for storage keys (TypeScript utility)
export type StorageKey = typeof STORAGE_KEYS[keyof typeof STORAGE_KEYS];
