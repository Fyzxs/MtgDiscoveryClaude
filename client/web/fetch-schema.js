import https from 'https';
import fs from 'fs';

// Disable SSL verification for self-signed certificate
const agent = new https.Agent({
  rejectUnauthorized: false
});

const options = {
  hostname: 'localhost',
  port: 65203,
  path: '/graphql?sdl',
  method: 'GET',
  agent: agent
};

console.log('Fetching GraphQL schema from https://localhost:65203/graphql?sdl...');

const req = https.request(options, (res) => {
  let responseData = '';

  res.on('data', (chunk) => {
    responseData += chunk;
  });

  res.on('end', () => {
    if (res.statusCode === 200) {
      fs.writeFileSync('schema.graphql', responseData);
      console.log('✓ Schema saved to schema.graphql');
    } else {
      console.error('✗ Error fetching schema. Status code:', res.statusCode);
      console.error('Response:', responseData);
      process.exit(1);
    }
  });
});

req.on('error', (error) => {
  console.error('✗ Error fetching schema:', error);
  process.exit(1);
});

req.end();
