import { gql } from '@apollo/client';

export const DELETE_COLLECTION = gql`
  mutation DeleteCollection($collectionId: String!) {
    deleteCollection(args: { collectionId: $collectionId }) {
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
