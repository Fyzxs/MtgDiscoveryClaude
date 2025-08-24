import type { CodegenConfig } from '@graphql-codegen/cli';

const config: CodegenConfig = {
  schema: 'https://localhost:65203/graphql',
  documents: ['src/**/*.{ts,tsx}'],
  ignoreNoDocuments: true,
  generates: {
    './src/generated/': {
      preset: 'client',
      presetConfig: {
        gqlTagName: 'gql',
      },
      plugins: [],
      config: {
        useTypeImports: true,
        skipTypename: false,
        withHooks: true,
        withHOC: false,
        withComponent: false,
      },
    },
  },
};

export default config;