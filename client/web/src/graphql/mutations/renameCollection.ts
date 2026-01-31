import { gql } from '@apollo/client';

export const RENAME_COLLECTION = gql`
  mutation RenameCollection($collectionId: String!, $name: String!) {
    renameCollection(args: { collectionId: $collectionId, name: $name }) {
      __typename
      ... on CollectionsSuccessResponse {
        data {
          collectionId
          name
          updatedAt
        }
      }
      ... on FailureResponse {
        status {
          message
          statusCode
        }
      }
    }
  }
`;
