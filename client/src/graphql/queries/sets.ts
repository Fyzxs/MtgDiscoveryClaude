import { gql } from '@apollo/client';

export const GET_ALL_SETS = gql`
  query GetAllSets {
    allSets {
      __typename
      ... on SuccessSetsResponse {
        data {
          id
          code
          tcgPlayerId
          name
          uri
          scryfallUri
          searchUri
          releasedAt
          setType
          cardCount
          printedSize
          digital
          nonFoilOnly
          foilOnly
          block
          iconSvgUri
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

export const GET_SETS_BY_CODE = gql`
  query GetMultipleSetsByCode($codes: SetCodesArgEntityInput!) {
    setsByCode(codes: $codes) {
      __typename
      ... on SuccessSetsResponse {
        data {
          id
          code
          tcgPlayerId
          name
          uri
          scryfallUri
          searchUri
          releasedAt
          setType
          cardCount
          printedSize
          digital
          nonFoilOnly
          foilOnly
          block
          iconSvgUri
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