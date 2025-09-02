import { gql } from '@apollo/client';

export const ARTIST_NAME_SEARCH = gql`
  query ArtistSearch($searchTerm: ArtistSearchTermArgEntityInput!) {
    artistSearch(searchTerm: $searchTerm) {
      __typename
      ... on SuccessArtistSearchResultsResponse {
        data {
          artistId
          name
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