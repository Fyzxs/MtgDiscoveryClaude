import { gql } from '@apollo/client';

export const ARTIST_NAME_SEARCH = gql`
  query ArtistNameSearch($searchTerm: ArtistSearchTermArgEntityInput!) {
    artistNameSearch(searchTerm: $searchTerm) {
      __typename
      ... on SuccessArtistNameSearchResultsResponse {
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