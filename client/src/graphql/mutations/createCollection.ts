import { gql } from '@apollo/client';

export const CREATE_COLLECTION = gql`
  mutation CreateCollection($name: String!, $type: String!, $visibility: String!) {
    createCollection(args: { name: $name, type: $type, visibility: $visibility }) {
      __typename
      ... on CollectionsSuccessResponse {
        data {
          collectionId
          ownerId
          name
          type
          visibility
          isDefault
          createdAt
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
