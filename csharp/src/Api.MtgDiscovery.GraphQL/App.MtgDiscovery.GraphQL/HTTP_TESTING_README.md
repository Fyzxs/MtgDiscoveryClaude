# HTTP Testing with httpYac

This directory contains `.http` files for testing the GraphQL API endpoints using httpYac or similar HTTP file runners.

## Setup

### 1. Install httpYac Extension
- **VS Code**: Install the "httpYac - Rest Client" extension
- **JetBrains IDEs**: Install the "HTTP Client" plugin
- **CLI**: `npm install -g httpyac`

### 2. Configure Environment Variables

1. Copy the example environment file:
   ```bash
   cp .env.example .env
   ```

2. Edit `.env` and add your JWT token:
   ```env
   BEARER_TOKEN=your_actual_jwt_token_here
   ```

3. **IMPORTANT**: Never commit the `.env` file to source control!

### 3. Getting a JWT Token

To get a valid JWT token for testing:

1. Go to your Auth0 dashboard (https://manage.auth0.com/)
2. Navigate to Applications → Applications and find your SPA app
3. Use Auth0's test tab or implement frontend login flow
4. Copy the JWT token from the response
5. Paste it into your `.env` file

Required JWT Claims:
- `sub`: User's unique identifier (e.g., "auth0|68b7c2c1b773c5897e8ceb8f")
- `nickname`: User's display name (e.g., "quinntgil+mtg01")

## Available Test Files

### Queries (No Authentication Required)
- `allsets-queries.http` - Test set listing endpoints
- `card-queries.http` - Test card retrieval endpoints
- `cardNameSearch-queries.http` - Test card search functionality
- `cardsByArtist-queries.http` - Test artist-based card queries
- `cardsByArtistName-queries.http` - Test artist name searches
- `cardsByName-queries.http` - Test card name searches
- `cardsBySetCode-queries.http` - Test set-based card queries
- `set-queries.http` - Test set information endpoints
- `artistSearch-queries.http` - Test artist search functionality

### Mutations (Authentication Required)
- `registerUser-mutation.http` - Test user registration
- `addCardToCollection-mutation.http` - Test adding cards to user collection

### Authentication Testing
- `graphql-auth0-test.http` - Test Auth0 integration

## Running Tests

### VS Code with httpYac
1. Open any `.http` file
2. Click "Send Request" above each request
3. View responses in the output panel

### Command Line
```bash
# Run all tests
httpyac send *.http

# Run specific file
httpyac send card-queries.http

# Run with specific environment
httpyac send --env=dev card-queries.http
```

### JetBrains IDEs
1. Open any `.http` file
2. Click the green arrow next to each request
3. View responses in the HTTP Response panel

## Environment Management

The `.httpyac.json` file defines multiple environments:
- `local` (default): https://localhost:65203
- `dev`: Development server
- `staging`: Staging server
- `production`: Production server

To switch environments:
```bash
httpyac send --env=staging card-queries.http
```

## Security Notes

1. **Never commit `.env` files** containing real JWT tokens
2. **Rotate JWT tokens regularly** for security
3. **Use environment-specific tokens** for different environments
4. The `.env` file is already added to `.gitignore`
5. Share tokens securely through password managers or secure channels

## Troubleshooting

### "Unauthorized" Errors
- Check that your JWT token in `.env` is valid and not expired
- Verify the token contains required claims (`sub`, `nickname`)
- Ensure the `Authorization: Bearer {{bearerToken}}` header is present

### Connection Errors
- Verify the GraphQL server is running on the configured port
- Check `BASE_URL` in `.env` matches your server configuration
- For HTTPS, you may need to accept self-signed certificates

### Variable Not Found
- Ensure `.env` file exists and contains all required variables
- Check that httpYac dotenv support is enabled in `.httpyac.json`
- Verify variable names match between `.env` and `.http` files

## Contributing

When adding new `.http` files:
1. Use environment variables for all URLs and secrets
2. Add descriptive comments for each request
3. Include example responses in comments
4. Group related requests with `###` separators
5. Follow the existing naming convention: `{feature}-{type}.http`