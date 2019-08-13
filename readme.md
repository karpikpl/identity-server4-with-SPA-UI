# About
Identity Server 4 sample with SPA login page

# Projects

## client
Client app that authenticates user against IdentityServer4
Runs on port 8080.

## id4
Identity Server 4 API.
Runs on port 5000

## login-spa
Login page (UI) for IdentityServer4.
Runs on port 8082

## api
Represents protected resource that client is trying to access.
Runs on port 8083

# How to run
## client
```bash
client> npx http-server
```
## id4
```bash
id4\src\IdentityServer> dotnet run
```
## login-spa
```bash
login-spa> npx http-server -p 8082
```
## api
```bash
api> dotnet run
```

# To test
1. Navigate your browser to http://localhost:8080/index.html
2. Use alice/alice or bob/bob as username and password

# Docker
```
docker-compose up
```
