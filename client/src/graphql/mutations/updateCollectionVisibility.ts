import { gql } from '@apollo/client';

export const UPDATE_COLLECTION_VISIBILITY = gql`
  mutation UpdateCollectionVisibility($collectionId: String!, $visibility: String!) {
    updateCollectionVisibility(args: { collectionId: $collectionId, visibility: $visibility }) {
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
