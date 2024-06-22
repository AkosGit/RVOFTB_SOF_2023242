# SnipSmart

## Technologies used
- Client: Vue3, Pinia, Native-ui, codemirror, axios
- Server: ASP.NET 8.0 Web API, SQLite database

## Setting up
- Build with docker compose
    - rewrite baseURL ip/domain on client to reach server (yea should have been done with ENV): /SnipSmartClient/src/clients/BaseClient.ts
    - Client: /SnipSmartClient/docker-compose.yml
    - Server: /SnipSmart/docker-compose.yml
    - Client will be at port 90 server at 8089

