# Navigation Drawer Debugging Instructions

## Problem
The navigation drawer stays open when using "Jump to Set" feature (Enter key or button click).

## Debug Logging Added

I've added comprehensive console.log statements to trace the execution flow:

### Files Modified
1. `/mnt/d/src/MtgDiscoveryVibeWorkspace/MtgDiscoveryVibe/client/src/components/organisms/shared/NavigationDrawer.tsx`
2. `/mnt/d/src/MtgDiscoveryVibeWorkspace/MtgDiscoveryVibe/client/src/components/organisms/shared/Header.tsx`

## Testing Steps

1. **Open the development console** (F12 in most browsers)
2. **Open the mobile navigation drawer** (click hamburger menu)
3. **Type a set code** in the "Jump to Set" field (e.g., "lea")
4. **Press Enter** (or click the arrow button)
5. **Watch the console logs**

## Expected Console Output

You should see logs in this order:

```
[Header] Component rendering, mobileMenuOpen = true
[NavigationDrawer] handleSetCodeSubmit called with setCode: lea
[NavigationDrawer] About to call onClose()
[NavigationDrawer] onClose() called, about to navigate to: /set/lea
[Header] onClose callback called, setting mobileMenuOpen to false
[Header] Component rendering, mobileMenuOpen = false
[NavigationDrawer] setTimeout executing, calling navigateWithCollector
[Header] Component rendering, mobileMenuOpen = <true or false>
```

## What to Look For

1. **Is onClose() being called?** Look for `[NavigationDrawer] onClose() called`
2. **Is the Header callback triggered?** Look for `[Header] onClose callback called`
3. **Is mobileMenuOpen changing to false?** Look for `[Header] Component rendering, mobileMenuOpen = false`
4. **Does it stay false after navigation?** Check the last render log

## Test Comparison

Also test clicking a regular navigation item (e.g., "All Sets") and compare the logs:

```
[NavigationDrawer] handleNavigation called with path: /sets
[NavigationDrawer] onClose() called
[Header] onClose callback called, setting mobileMenuOpen to false
[NavigationDrawer] navigateWithCollector() called
[Header] Component rendering, mobileMenuOpen = false
```

## Possible Issues to Identify

1. **onClose not called**: The function isn't being executed
2. **onClose called but state doesn't update**: React state batching issue
3. **State updates but reverts**: Something is resetting the state after close
4. **Header re-renders with wrong state**: Route change is interfering

## Report Back

Please run the test and copy/paste the **exact console output** so we can identify the root cause.
