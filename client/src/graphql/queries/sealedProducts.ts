import { gql } from '@apollo/client';

export const GET_SEALED_PRODUCTS_BY_SET_CODE = gql`
  query GetSealedProductsBySetCode($args: GetSealedProductsBySetCodeArgEntityInput!) {
    sealedProductsBySetCode(args: $args) {
      __typename
      ... on SealedProductsSuccessResponse {
        data {
          uuid
          setId
          setCode
          setName
          name
          category
          subtype
          cardCount
          releaseDate
          tcgplayerProductId
          imageUrl
          purchaseUrlTcgplayer
          purchaseUrlCardmarket
          purchaseUrlCardKingdom
          userQuantity
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
