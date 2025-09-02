# Azure B2C Authentication Implementation Guide

## Overview
Implement Azure B2C authentication for the MTG Discovery application, enabling user authentication for mutations while maintaining anonymous access for public queries.

## Azure B2C Configuration

### Development Environment Settings
```env
# B2C Tenant Configuration
TENANT_NAME=azmtgdiscovery
CLIENT_ID=b03f7be1-0bf1-43a4-b5ef-9fcfe93eb528
TENANT_ID=84035287-cbfd-4c3c-89a1-5c9b4f02b600

# User Flows/Policies
SIGNUP_SIGNIN_POLICY=B2C_1_signupsignin1
PASSWORD_RESET_POLICY=B2C_1_passwordreset1
EDIT_PROFILE_POLICY=B2C_1_editprofile1

# URLs
AUTHORITY=https://azmtgdiscovery.b2clogin.com
KNOWN_AUTHORITIES=azmtgdiscovery.b2clogin.com
REDIRECT_URI=http://localhost:5173
POST_LOGOUT_REDIRECT_URI=http://localhost:5173

# Scopes
API_SCOPES=openid offline_access
```

### Azure Portal Configuration Required
1. Navigate to Azure AD B2C → App registrations → MtgDiscovery app
2. Under Authentication → Single-page application:
   - Add redirect URI: `http://localhost:5173`
   - Ensure "Access tokens" and "ID tokens" are checked
3. Confirm API permissions show: `openid` and `offline_access`

---

## Frontend Implementation Tasks

### 1. Install Dependencies
```bash
cd client
npm install @azure/msal-browser @azure/msal-react
```

### 2. Create Environment Configuration
Create `/client/.env.development`:
```env
VITE_B2C_CLIENT_ID=b03f7be1-0bf1-43a4-b5ef-9fcfe93eb528
VITE_B2C_TENANT_NAME=azmtgdiscovery
VITE_B2C_AUTHORITY=https://azmtgdiscovery.b2clogin.com
VITE_B2C_KNOWN_AUTHORITIES=azmtgdiscovery.b2clogin.com
VITE_B2C_SIGNUP_SIGNIN_POLICY=B2C_1_signupsignin1
VITE_B2C_PASSWORD_RESET_POLICY=B2C_1_passwordreset1
VITE_B2C_EDIT_PROFILE_POLICY=B2C_1_editprofile1
VITE_B2C_REDIRECT_URI=http://localhost:5173
VITE_B2C_POST_LOGOUT_REDIRECT_URI=http://localhost:5173
VITE_B2C_API_SCOPES=openid offline_access
VITE_B2C_CACHE_LOCATION=sessionStorage
```

### 3. Create MSAL Configuration
Create `/client/src/auth/authConfig.ts`:
```typescript
import { Configuration, LogLevel } from '@azure/msal-browser';

export const b2cPolicies = {
  names: {
    signUpSignIn: import.meta.env.VITE_B2C_SIGNUP_SIGNIN_POLICY,
    passwordReset: import.meta.env.VITE_B2C_PASSWORD_RESET_POLICY,
    editProfile: import.meta.env.VITE_B2C_EDIT_PROFILE_POLICY
  },
  authorities: {
    signUpSignIn: {
      authority: `${import.meta.env.VITE_B2C_AUTHORITY}/${import.meta.env.VITE_B2C_TENANT_NAME}.onmicrosoft.com/${import.meta.env.VITE_B2C_SIGNUP_SIGNIN_POLICY}`
    },
    passwordReset: {
      authority: `${import.meta.env.VITE_B2C_AUTHORITY}/${import.meta.env.VITE_B2C_TENANT_NAME}.onmicrosoft.com/${import.meta.env.VITE_B2C_PASSWORD_RESET_POLICY}`
    },
    editProfile: {
      authority: `${import.meta.env.VITE_B2C_AUTHORITY}/${import.meta.env.VITE_B2C_TENANT_NAME}.onmicrosoft.com/${import.meta.env.VITE_B2C_EDIT_PROFILE_POLICY}`
    }
  }
};

export const msalConfig: Configuration = {
  auth: {
    clientId: import.meta.env.VITE_B2C_CLIENT_ID,
    authority: b2cPolicies.authorities.signUpSignIn.authority,
    knownAuthorities: [import.meta.env.VITE_B2C_KNOWN_AUTHORITIES],
    redirectUri: import.meta.env.VITE_B2C_REDIRECT_URI,
    postLogoutRedirectUri: import.meta.env.VITE_B2C_POST_LOGOUT_REDIRECT_URI,
    navigateToLoginRequestUrl: true
  },
  cache: {
    cacheLocation: import.meta.env.VITE_B2C_CACHE_LOCATION || 'sessionStorage',
    storeAuthStateInCookie: false
  },
  system: {
    loggerOptions: {
      loggerCallback: (level, message, containsPii) => {
        if (containsPii) return;
        switch (level) {
          case LogLevel.Error:
            console.error(message);
            return;
          case LogLevel.Warning:
            console.warn(message);
            return;
          default:
            return;
        }
      }
    }
  }
};

export const loginRequest = {
  scopes: import.meta.env.VITE_B2C_API_SCOPES?.split(' ') || ['openid', 'offline_access']
};
```

### 4. Create MSAL Instance
Create `/client/src/auth/msalInstance.ts`:
```typescript
import { PublicClientApplication, EventType, EventMessage, AuthenticationResult } from '@azure/msal-browser';
import { msalConfig, b2cPolicies } from './authConfig';

export const msalInstance = new PublicClientApplication(msalConfig);

// Handle the redirect promise
msalInstance.initialize().then(() => {
  msalInstance.handleRedirectPromise().then((response) => {
    if (response) {
      msalInstance.setActiveAccount(response.account);
    } else {
      const accounts = msalInstance.getAllAccounts();
      if (accounts.length > 0) {
        msalInstance.setActiveAccount(accounts[0]);
      }
    }
  }).catch((error) => {
    console.error('Redirect error:', error);
  });
});

// Listen for sign-in events
msalInstance.addEventCallback((event: EventMessage) => {
  if (event.eventType === EventType.LOGIN_SUCCESS && event.payload) {
    const payload = event.payload as AuthenticationResult;
    msalInstance.setActiveAccount(payload.account);
  }
  
  // Handle password reset flow
  if (event.eventType === EventType.LOGIN_FAILURE && event.error?.errorMessage?.includes('AADB2C90118')) {
    const resetAuthority = b2cPolicies.authorities.passwordReset.authority;
    msalInstance.loginRedirect({
      authority: resetAuthority,
      scopes: []
    });
  }
});
```

### 5. Create Authentication Hook
Create `/client/src/auth/hooks/useAuth.ts`:
```typescript
import { useMsal } from '@azure/msal-react';
import { loginRequest } from '../authConfig';
import { useCallback } from 'react';

export const useAuth = () => {
  const { instance, accounts, inProgress } = useMsal();
  const account = accounts[0];

  const login = useCallback(() => {
    instance.loginRedirect(loginRequest);
  }, [instance]);

  const logout = useCallback(() => {
    instance.logoutRedirect();
  }, [instance]);

  const getToken = useCallback(async () => {
    if (!account) return null;
    
    try {
      const response = await instance.acquireTokenSilent({
        ...loginRequest,
        account
      });
      return response.idToken;
    } catch (error) {
      // Fall back to interactive token acquisition
      const response = await instance.acquireTokenRedirect(loginRequest);
      return response?.idToken || null;
    }
  }, [instance, account]);

  return {
    isAuthenticated: !!account,
    account,
    login,
    logout,
    getToken,
    inProgress
  };
};
```

### 6. Wrap App with MSAL Provider
Update `/client/src/main.tsx`:
```typescript
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { ApolloProvider } from '@apollo/client/react'
import { MsalProvider } from '@azure/msal-react'
import { ThemeProvider } from '@mui/material/styles'
import CssBaseline from '@mui/material/CssBaseline'
import './index.css'
import App from './App.tsx'
import { apolloClient } from './graphql/apollo-client'
import { theme } from './theme'
import { ErrorBoundary } from './components/ErrorBoundary'
import { msalInstance } from './auth/msalInstance'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ErrorBoundary level="page" name="Root">
      <MsalProvider instance={msalInstance}>
        <ThemeProvider theme={theme}>
          <CssBaseline />
          <ApolloProvider client={apolloClient}>
            <App />
          </ApolloProvider>
        </ThemeProvider>
      </MsalProvider>
    </ErrorBoundary>
  </StrictMode>,
)
```

### 7. Update Apollo Client with Auth Link
Update `/client/src/graphql/apollo-client.ts`:
```typescript
import { ApolloClient, InMemoryCache, createHttpLink, ApolloLink } from '@apollo/client';
import { setContext } from '@apollo/client/link/context';
import { msalInstance } from '../auth/msalInstance';
import { loginRequest } from '../auth/authConfig';

const httpLink = createHttpLink({
  uri: 'http://localhost:5205/graphql'
});

const authLink = setContext(async (_, { headers }) => {
  const account = msalInstance.getActiveAccount();
  
  if (account) {
    try {
      const response = await msalInstance.acquireTokenSilent({
        ...loginRequest,
        account
      });
      
      return {
        headers: {
          ...headers,
          authorization: response.idToken ? `Bearer ${response.idToken}` : ''
        }
      };
    } catch (error) {
      console.error('Token acquisition failed:', error);
    }
  }
  
  return { headers };
});

export const apolloClient = new ApolloClient({
  link: ApolloLink.from([authLink, httpLink]),
  cache: new InMemoryCache(),
  defaultOptions: {
    watchQuery: {
      errorPolicy: 'all'
    },
    query: {
      errorPolicy: 'all'
    }
  }
});
```

### 8. Create Login/Logout Component
Create `/client/src/components/auth/AuthButton.tsx`:
```typescript
import { Button } from '@mui/material';
import { useAuth } from '../../auth/hooks/useAuth';

export const AuthButton = () => {
  const { isAuthenticated, account, login, logout, inProgress } = useAuth();

  if (inProgress === 'login' || inProgress === 'logout') {
    return <Button disabled>Loading...</Button>;
  }

  if (isAuthenticated && account) {
    return (
      <div style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
        <span>Welcome, {account.name || account.username}</span>
        <Button onClick={logout} variant="outlined">
          Logout
        </Button>
      </div>
    );
  }

  return (
    <Button onClick={login} variant="contained">
      Login
    </Button>
  );
};
```

### 9. Add Auth Button to Layout
Update the Layout component to include the AuthButton in the header.

---

## Backend Implementation Tasks

### 1. Install NuGet Package
```bash
cd src/App.MtgDiscovery.GraphQL
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

### 2. Add Configuration to appsettings.json
Update `/src/App.MtgDiscovery.GraphQL/appsettings.json`:
```json
{
  "AzureAdB2C": {
    "Instance": "https://azmtgdiscovery.b2clogin.com",
    "Domain": "azmtgdiscovery.onmicrosoft.com",
    "ClientId": "b03f7be1-0bf1-43a4-b5ef-9fcfe93eb528",
    "SignUpSignInPolicyId": "B2C_1_signupsignin1"
  }
}
```

### 3. Configure Authentication in Program.cs
Add to `/src/App.MtgDiscovery.GraphQL/Program.cs`:
```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

// Add after builder creation
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var b2cConfig = builder.Configuration.GetSection("AzureAdB2C");
        var instance = b2cConfig["Instance"];
        var domain = b2cConfig["Domain"];
        var policy = b2cConfig["SignUpSignInPolicyId"];
        var clientId = b2cConfig["ClientId"];
        
        options.Authority = $"{instance}/{domain}/{policy}/v2.0/";
        options.Audience = clientId;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

// Add after app creation, before GraphQL middleware
app.UseAuthentication();
app.UseAuthorization();
```

### 4. Access User Claims in GraphQL
Example for accessing user information in queries/mutations:
```csharp
public class UserQueries
{
    [GraphQLDescription("Get current user information")]
    public UserInfo? GetCurrentUser(ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal?.Identity?.IsAuthenticated != true)
            return null;
            
        return new UserInfo
        {
            Id = claimsPrincipal.FindFirst("sub")?.Value,
            Email = claimsPrincipal.FindFirst("emails")?.Value,
            Name = claimsPrincipal.FindFirst("name")?.Value
        };
    }
}

public class ProtectedMutations
{
    [Authorize] // Requires authentication
    [GraphQLDescription("Save a card to user's collection")]
    public async Task<bool> SaveCardToCollection(
        string cardId,
        ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirst("sub")?.Value;
        // Implementation here
        return true;
    }
}
```

---

## Testing

### 1. Manual Token Acquisition
Create `/test/get-token.http`:
```http
### Get B2C Token Manually
POST https://azmtgdiscovery.b2clogin.com/azmtgdiscovery.onmicrosoft.com/B2C_1_signupsignin1/oauth2/v2.0/token
Content-Type: application/x-www-form-urlencoded

grant_type=password
&client_id=b03f7be1-0bf1-43a4-b5ef-9fcfe93eb528
&scope=openid offline_access
&username=testuser@yourdomain.com
&password=YourPassword123!

### Extract the id_token from response for testing
```

### 2. Test GraphQL with Authentication
Create `/test/graphql-auth-test.http`:
```http
@baseUrl = http://localhost:5205/graphql
@token = YOUR_ID_TOKEN_HERE

### Test Public Query (No Auth Required)
POST {{baseUrl}}
Content-Type: application/json

{
  "query": "{ sets { name code } }"
}

### Test Authenticated Query
POST {{baseUrl}}
Content-Type: application/json
Authorization: Bearer {{token}}

{
  "query": "{ getCurrentUser { id email name } }"
}

### Test Protected Mutation
POST {{baseUrl}}
Content-Type: application/json
Authorization: Bearer {{token}}

{
  "query": "mutation { saveCardToCollection(cardId: \"test-123\") }"
}

### Test Unauthorized Access (Should Return 401)
POST {{baseUrl}}
Content-Type: application/json

{
  "query": "mutation { saveCardToCollection(cardId: \"test-123\") }"
}
```

### 3. Decode and Verify Token
Use https://jwt.ms to decode tokens and verify:
- `iss`: Should be `https://azmtgdiscovery.b2clogin.com/{tenant-id}/v2.0/`
- `aud`: Should be `b03f7be1-0bf1-43a4-b5ef-9fcfe93eb528`
- `exp`: Token expiration time
- User claims: `sub`, `name`, `emails`

---

## Acceptance Criteria

### Frontend
- [ ] User can click login button and be redirected to B2C
- [ ] After login, user sees their name/email
- [ ] User can logout successfully
- [ ] Token is automatically attached to GraphQL requests when available
- [ ] App handles token expiration gracefully
- [ ] Password reset flow works correctly

### Backend
- [ ] Public queries work without authentication
- [ ] Protected mutations require valid token
- [ ] Invalid tokens return 401 Unauthorized
- [ ] User claims are accessible in GraphQL resolvers
- [ ] Token validation happens automatically

### Integration
- [ ] Frontend successfully sends authenticated requests
- [ ] Backend correctly validates B2C tokens
- [ ] Error messages are user-friendly
- [ ] Token refresh happens seamlessly

---

## Troubleshooting

### Common Issues

1. **CORS Errors**: Ensure GraphQL endpoint allows `http://localhost:5173`
2. **Token Validation Fails**: Check authority URL matches exactly
3. **Redirect Issues**: Verify redirect URIs in Azure portal match exactly
4. **Silent Token Acquisition Fails**: Check cache location setting
5. **Password Reset Loop**: Ensure AADB2C90118 error code handling is implemented

### Debug Endpoints
- B2C Metadata: `https://azmtgdiscovery.b2clogin.com/azmtgdiscovery.onmicrosoft.com/B2C_1_signupsignin1/v2.0/.well-known/openid-configuration`
- JWKS URI: Check metadata endpoint for current keys URL

---

## Production Considerations

1. Update redirect URIs to production domain
2. Use environment-specific configuration files
3. Enable HTTPS for all endpoints
4. Consider implementing refresh token rotation
5. Add monitoring for authentication failures
6. Implement proper error logging without exposing sensitive data