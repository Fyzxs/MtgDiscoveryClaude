import { gql } from '@apollo/client';

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