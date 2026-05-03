const { env } = require('node:process');

let target = 'https://localhost:7079';

if (env.ASPNETCORE_HTTPS_PORT) {
  target = `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`;
} else if (env.ASPNETCORE_URLS) {
  target = env.ASPNETCORE_URLS.split(';')[0];
}

const PROXY_CONFIG = [
  {
    context: [
      "/api",
      "/health",
    ],
    target,
    secure: false
  }
]

module.exports = PROXY_CONFIG;
