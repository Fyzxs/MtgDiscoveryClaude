import { gql } from '@apollo/client';

export const GET_USER_CARDS_BY_SET = gql`
  query GetUserCardsBySet($setArgs: UserCardsSetArgEntityInput!) {
    userCardsBySet(setArgs: $setArgs) {
      __typename
      ... on SuccessUserCardsCollectionResponse {
        data {
          userId
          cardId
          setId
          collectedList {
            finish
            special
            count
          }
        }
        status {
          message
          statusCode
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