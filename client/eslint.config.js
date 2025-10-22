import js from '@eslint/js'
import globals from 'globals'
import reactHooks from 'eslint-plugin-react-hooks'
import reactRefresh from 'eslint-plugin-react-refresh'
import tseslint from 'typescript-eslint'

export default tseslint.config(
  { ignores: ['dist'] },
  {
    extends: [js.configs.recommended, ...tseslint.configs.recommended],
    files: ['**/*.{ts,tsx}'],
    languageOptions: {
      ecmaVersion: 2020,
      globals: globals.browser,
    },
    plugins: {
      'react-hooks': reactHooks,
      'react-refresh': reactRefresh,
    },
    rules: {
      ...reactHooks.configs.recommended.rules,
      'react-refresh/only-export-components': [
        'warn',
        { allowConstantExport: true },
      ],
      // Enforce use of logger utility instead of console.log
      'no-console': [
        'warn',
        {
          allow: ['warn', 'error'], // Allow console.warn and console.error
        },
      ],
    },
  },
  // Enforce atomic design hierarchy: pages and templates should NOT import atoms directly
  // Exceptions: type-only imports, icons, and accessibility components
  {
    files: ['src/components/pages/**/*.{ts,tsx}', 'src/components/templates/**/*.{ts,tsx}'],
    rules: {
      'no-restricted-imports': [
        'warn', // Changed to 'warn' to allow documented exceptions
        {
          patterns: [
            {
              group: ['../atoms', '../atoms/*', '../../atoms', '../../atoms/*'],
              message: 'Pages and templates should NOT import atoms directly. Use molecules or organisms instead. Exceptions: type-only imports (SxProps, Theme), icons, and specialized components (SkipNavigation) are allowed as documented in ATOMIC_DESIGN_HIERARCHY.md',
            },
          ],
        },
      ],
    },
  },
)
