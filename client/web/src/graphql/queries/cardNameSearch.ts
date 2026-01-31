import { gql } from '@apollo/client';

export const CARD_NAME_SEARCH = gql`
  query CardNameSearch($searchTerm: CardSearchTermInput!) {
    cardNameSearch(searchTerm: $searchTerm) {
      __typename
      ... on CardNameSearchResultsSuccessResponse {
        data {
          name
        }
      }
      ... on FailureResponse {
        status {
          message
        }
      }
    }
  }
`;